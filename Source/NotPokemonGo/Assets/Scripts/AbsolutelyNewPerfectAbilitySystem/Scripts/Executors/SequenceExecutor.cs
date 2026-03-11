using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class SequenceExecutor
    {
        private StepExecutorRegistry _registry;

        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var seq = (SequenceStep)step;

            foreach (var child in seq.Steps)
            {
                await _registry.Execute(child, ctx);
            }
        }
    }
}