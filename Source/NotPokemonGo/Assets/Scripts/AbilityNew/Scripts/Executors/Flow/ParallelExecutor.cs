using System.Collections.Generic;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class ParallelExecutor : AbilityStepExecutor<ParallelStep>
    {
        private StepExecutorRegistry _registry;

        public ParallelExecutor(StepExecutorRegistry registry) => 
            _registry = registry;

        public override async UniTask Execute(ParallelStep step, AbilityExecutionRuntime runtime)
        {
            var tasks = new List<UniTask>();
            
            foreach (var child in step.Steps) 
                tasks.Add(_registry.Execute(child, runtime));
            
            await UniTask.WhenAll(tasks);
        }
    }
}