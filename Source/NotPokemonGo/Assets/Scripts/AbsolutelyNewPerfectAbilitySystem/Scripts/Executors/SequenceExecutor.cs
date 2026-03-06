using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using AbsolutelyNewPerfectAbilitySystem.Steps;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Executors
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