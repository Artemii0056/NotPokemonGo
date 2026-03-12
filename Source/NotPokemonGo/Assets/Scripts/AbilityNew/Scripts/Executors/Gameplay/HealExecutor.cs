using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Executors;
using AbilityNew.Scripts.Results;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using Effects;
using Platoons;
using Units;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class HealStepExecutor : AbilityStepExecutor<HealStep>
    {
        private readonly IEffectResolver _effectResolver;
        private readonly ITargetSelector _targetSelector;

        public HealStepExecutor(
            IEffectResolver effectResolver,
            ITargetSelector targetSelector)
        {
            _effectResolver = effectResolver;
            _targetSelector = targetSelector;
        }

        public override UniTask Execute(HealStep step, AbilityExecutionRuntime runtime)
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
                    new HealAppliedBattleEvent(
                        runtime.Context.Source,
                        target,
                        step.Effect.Value));
            }

            return UniTask.CompletedTask;
        }
    }
}