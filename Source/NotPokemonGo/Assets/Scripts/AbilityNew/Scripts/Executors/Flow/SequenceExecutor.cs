using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class SequenceExecutor : AbilityStepExecutor<SequenceStep>
    {
        private StepExecutorRegistry _registry;
        
        public override async UniTask Execute(SequenceStep step, AbilityExecutionRuntime runtime)
        {
            foreach (var child in step.Steps) 
                await _registry.Execute(child, runtime);
        }
    }
}