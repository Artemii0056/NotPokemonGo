using System;
using System.Threading;
using Abilities.Signals;
using Armaments.Movers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Abilities.Flow
{
    public sealed class LaunchPreparedMoversOnAttackStep : IAbilityStep
    {
        private readonly float _delayBetweenShots;
        private readonly Func<AbilityExecutionContext, float> _delayProvider;
        private readonly Func<AbilityExecutionContext, IArmamentMover, bool> _skipImpact;

        public LaunchPreparedMoversOnAttackStep(
            float delayBetweenShots,
            Func<AbilityExecutionContext, IArmamentMover, bool> skipImpact = null)
        {
            _delayBetweenShots = delayBetweenShots;
            _delayProvider = null;
            _skipImpact = skipImpact;
        }

        public LaunchPreparedMoversOnAttackStep(
            Func<AbilityExecutionContext, float> delayProvider,
            Func<AbilityExecutionContext, IArmamentMover, bool> skipImpact = null)
        {
            _delayBetweenShots = 0f;
            _delayProvider = delayProvider;
            _skipImpact = skipImpact;
        }

        public async UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            bool gotAttack = await WaitForSignalOrTimeout(ctx.Source.AnimatorTrigger, PhaseSignal.Attack1, 0.75f, token);
            if (!gotAttack)
                Debug.LogWarning("[LaunchPreparedMovers] Attack1 signal missing, launching shots by timeout fallback.");

            ctx.TotalShots = ctx.PreparedMovers.Count;

            float delay = _delayProvider != null ? _delayProvider(ctx) : _delayBetweenShots;

            for (int i = 0; i < ctx.PreparedMovers.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                var mover = ctx.PreparedMovers[i];
                RegisterMover(ctx, mover);

                mover.Move();

                if (delay > 0f)
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            }

            await UniTask.WaitUntil(() => ctx.ActiveMovers.Count == 0, cancellationToken: token);

            ctx.PreparedMovers.Clear();
        }

        private void RegisterMover(AbilityExecutionContext ctx, IArmamentMover mover)
        {
            ctx.ActiveMovers.Add(mover);

            // IMPORTANT: gate token must be disposed EXACTLY ONCE
            var gateToken = ctx.PhaseService.Acquire($"Shot:{(mover.Armament != null ? mover.Armament.name : "null")}");

            bool finished = false;

            void Finish(IArmamentMover m, bool resolveImpact)
            {
                if (finished) return;
                finished = true;

                m.Reached -= OnReached;

                if (resolveImpact)
                    ctx.ImpactResolver.Resolve(m);

                if (m.Armament != null)
                    Object.Destroy(m.Armament.gameObject);

                ctx.ActiveMovers.Remove(m);
                gateToken.Dispose();
            }

            void OnReached(IArmamentMover m)
            {
                bool skip = _skipImpact != null && _skipImpact(ctx, m);
                Finish(m, resolveImpact: !skip);
            }

            mover.Reached += OnReached;

            ctx.AddCleanup(() =>
            {
                try { mover.Reached -= OnReached; } catch { }

                if (!finished)
                {
                        try
                        {
                            if (mover.Armament != null)
                                Object.Destroy(mover.Armament.gameObject);
                        }
                        catch { }

                        ctx.ActiveMovers.Remove(mover);
                        gateToken.Dispose();
                    }
            });
        }

        private static async UniTask<bool> WaitForSignalOrTimeout(
            Units.AnimationControllers.UnitAnimatorTrigger trigger,
            PhaseSignal signal,
            float timeoutSeconds,
            CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            void OnRaised(PhaseSignal s)
            {
                if (s != signal) return;
                tcs.TrySetResult();
            }

            trigger.SignalRaised += OnRaised;

            try
            {
                var timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(timeoutSeconds), cancellationToken: token);

                int winIndex = await UniTask.WhenAny(tcs.Task, timeoutTask);

                return winIndex == 0; // 0 = сигнал, 1 = таймаут
            }
            finally
            {
                trigger.SignalRaised -= OnRaised;
            }
        }
    }
}