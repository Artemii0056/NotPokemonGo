using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Results;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Statuses;
using Statuses.Services;
using Units;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class ApplyStatusStepExecutor : AbilityStepExecutor<ApplyStatusStep>
    {
        private readonly ITargetSelector _targetSelector;
        private readonly IStatusResolver _statusResolver;
        private readonly IStatusFactory _statusFactory;

        public ApplyStatusStepExecutor(
            ITargetSelector targetSelector,
            IStatusResolver statusResolver,
            IStatusFactory statusFactory)
        {
            _targetSelector = targetSelector;
            _statusResolver = statusResolver;
            _statusFactory = statusFactory;
        }

        public override UniTask Execute(ApplyStatusStep step, AbilityExecutionRuntime runtime)
        {
            var targets = _targetSelector.GetTargets(
                step.TargetMode,
                runtime.Context.Source,
                runtime.Context.Target);

            if (targets == null || targets.Count == 0)
                return UniTask.CompletedTask;

            foreach (var target in targets)
            {
                if (target == null || target.IsAlive == false)
                    continue;

                var status = _statusFactory.Create(step.Status, target);
                _statusResolver.Resolve(status, target);

                runtime.Result.AddEvent(
                    new StatusAppliedBattleEvent(
                        runtime.Context.Source,
                        target,
                        step.Status.Type));
            }

            return UniTask.CompletedTask;
        }
    }
}