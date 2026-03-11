using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class RepeatExecutor : IAbilityStepExecutor
    {
        private readonly StepExecutorRegistry _registry;

        public RepeatExecutor(StepExecutorRegistry registry) => 
            _registry = registry;

        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var repeat = (RepeatStep)step;

            for (int i = 0; i < repeat.Count; i++) 
                await _registry.Execute(repeat.Step, ctx);
        }
    }
}