using System.Collections.Generic;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class ParallelExecutor : AbilityStepExecutor<ParallelStep>
    {
        private readonly StepExecutorRegistry _registry;

        public ParallelExecutor(StepExecutorRegistry registry) =>
            _registry = registry;

        public override async UniTask Execute(ParallelStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            if (step == null || step.Steps == null || step.Steps.Count == 0)
                return;

            var tasks = new List<UniTask>(step.Steps.Count);

            foreach (var child in step.Steps)
            {
                if (child == null)
                    continue;

                tasks.Add(_registry.Execute(child, runtime));
            }

            if (tasks.Count == 0)
                return;

            await UniTask.WhenAll(tasks);
        }
    }
}