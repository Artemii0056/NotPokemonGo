using System;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class WaitingExecutor : AbilityStepExecutor<WaitingStep>
    {
        public override async UniTask Execute(WaitingStep step, AbilityExecutionRuntime runtime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(step.Duration));
        }
    }
}