using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class SequenceExecutor : AbilityStepExecutor<SequenceStep>
    {
        private StepExecutorRegistry _registry;

        public SequenceExecutor(StepExecutorRegistry registry) =>
            _registry = registry;

        public override async UniTask Execute(SequenceStep step, AbilityExecutionRuntime runtime)
        {
            if (step == null || step.Steps == null || step.Steps.Count == 0)
                return;

            foreach (var child in step.Steps)
            {
                if (child == null)
                    continue;

                if (runtime.State.IsInterrupted || runtime.State.IsCancelled)
                    return;

                await _registry.Execute(child, runtime);
            }
        }
    }
}