using System;
using System.Threading;
using System.Threading.Tasks;
using Abilities.Configs;
using Cysharp.Threading.Tasks;

namespace Abilities.Flow.Steps
{
    public sealed class PlayPhaseStep : IAbilityStep
    {
        private readonly AbilityPhase _phase;

        public PlayPhaseStep(AbilityPhase phase) => _phase = phase;

        public async UniTask Execute(AbilityExecutionContext ctx, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var trigger = ctx.Source.AnimatorTrigger;
            var phaseService = trigger.PhaseService;
            var anim = ctx.Source.AnimatorController;

            trigger.SetTarget(ctx.Target);
            trigger.SetPhase(_phase);

            phaseService.BeginPhase(_phase, ctx.Scope.Id);

            anim.Play(_phase.AnimationCashName);

            float timeoutSeconds = anim.GetAnimationLength() + 0.75f;

            // IMPORTANT: convert to Task to allow safe WhenAny + await.
            Task completion = phaseService.WaitPhaseCompletionAsync(token).AsTask();
            Task timeout = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), token);

            Task winner = await Task.WhenAny(completion, timeout);

            if (winner == timeout && !token.IsCancellationRequested)
            {
                phaseService.ForceFinish($"Timeout phase={_phase.AnimationClip?.name} t={timeoutSeconds:0.##}s");
            }

            await completion;
        }
    }
}