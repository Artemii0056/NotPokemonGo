using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Executors;
using AbilityNew.Scripts.Results;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Platoons;
using Statuses.Services;
using Units;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class RemoveStatusStepExecutor : AbilityStepExecutor<RemoveStatusStep>
    {
        private readonly ITargetSelector _targetSelector;
        private readonly IStatusManager _statusManager;

        public RemoveStatusStepExecutor(
            ITargetSelector targetSelector,
            IStatusManager statusManager)
        {
            _targetSelector = targetSelector;
            _statusManager = statusManager;
        }

        public override UniTask Execute(RemoveStatusStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            var targets = _targetSelector.GetTargets(
                step.TargetMode,
                runtime.Context.Source,
                runtime.Context.Target);

            if (targets == null || targets.Count == 0)
                return UniTask.CompletedTask;

            foreach (var target in targets)
            {
                if (target == null)
                    continue;

                _statusManager.UnregisterStatus(target, step.StatusType);


                runtime.Result.AddEvent(
                    new StatusRemovedBattleEvent(
                        runtime.Context.Source,
                        target,
                        step.StatusType));
            }

            return UniTask.CompletedTask;
        }
    }
}
