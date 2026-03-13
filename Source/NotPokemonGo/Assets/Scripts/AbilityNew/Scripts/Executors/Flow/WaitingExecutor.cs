using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class WaitingExecutor : AbilityStepExecutor<WaitingStep>
    {
        public override async UniTask Execute(WaitingStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(step.Duration));
        }
    }
}