using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class RepeatExecutor : AbilityStepExecutor<RepeatStep>
    {
        private readonly StepExecutorRegistry _registry;

        public RepeatExecutor(StepExecutorRegistry registry) => 
            _registry = registry;
        
        public override async UniTask Execute(RepeatStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            for (int i = 0; i < step.Count; i++) 
                await _registry.Execute(step.Step, runtime);
        }
    }
}