using System.Threading;
using Abilities.Signals;
using Cysharp.Threading.Tasks;
using QteSystem;
using QteSystem.TestQte;

namespace Abilities.Flow.Steps
{
    /// <summary>
    /// MVP: IQteSession exposes only Completed/Result, so we apply shield once on Normal/Perfect.
    /// Session is disposed when phase sends Finish.
    /// </summary>
    public sealed class QteShieldUntilFinishStep : IAbilityStep
    {
        private readonly QteType _qteType;
        private readonly float _duration;
        private readonly IShieldApplier _shieldApplier;

        public QteShieldUntilFinishStep(QteType qteType, float duration, IShieldApplier shieldApplier)
        {
            _qteType = qteType;
            _duration = duration;
            _shieldApplier = shieldApplier;
        }

        public async UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            IQteSession session = ctx.QteService.StartSession(_qteType, ctx.Target, _duration);
            ctx.AddCleanup(() => session?.Dispose());

            void OnCompleted(QteResult result)
            {
                if (result == QteResult.Normal || result == QteResult.Perfect)
                    _shieldApplier.ApplyShield(ctx.Source);
            }

            session.Completed += OnCompleted;
            ctx.AddCleanup(() => session.Completed -= OnCompleted);

            // Wait until animator sends Finish for current phase
            await WaitForSignal(ctx.Source.AnimatorTrigger, PhaseSignal.Finish, token);

            session.Dispose();
        }

        private static async UniTask WaitForSignal(Units.AnimationControllers.UnitAnimatorTrigger trigger, PhaseSignal signal, CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource();

            void OnRaised(PhaseSignal s)
            {
                if (s != signal) return;
                trigger.SignalRaised -= OnRaised;
                tcs.TrySetResult();
            }

            trigger.SignalRaised += OnRaised;
            try { await tcs.Task.AttachExternalCancellation(token); }
            finally { trigger.SignalRaised -= OnRaised; }
        }
    }
}
