using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Results;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Effects;
using Units;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class DamageStepExecutor : AbilityStepExecutor<DamageStep>
    {
        private readonly IEffectResolver _effectResolver;
        private readonly ITargetSelector _targetSelector;

        public DamageStepExecutor(
            IEffectResolver effectResolver,
            ITargetSelector targetSelector)
        {
            _effectResolver = effectResolver;
            _targetSelector = targetSelector;
        }

        public override UniTask Execute(DamageStep step, AbilityExecutionRuntime runtime)
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

                _effectResolver.ApplyEffect(target, step.Effect);

                runtime.Result.AddEvent(
                    new DamageAppliedBattleEvent(
                        runtime.Context.Source,
                        target,
                        step.Effect.Value));
            }

            return UniTask.CompletedTask;
        }
    }
}