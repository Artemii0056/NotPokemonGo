using System.Collections.Generic;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class ParallelExecutor : IAbilityStepExecutor
    {
        private StepExecutorRegistry _registry;

        public ParallelExecutor(StepExecutorRegistry registry)
        {
            _registry = registry;
        }

        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var parallel = (ParallelStep)step;

            var tasks = new List<UniTask>();

            foreach (var child in parallel.Steps) 
                tasks.Add(_registry.Execute(child, ctx));

            await UniTask.WhenAll(tasks);
        }
    }
}