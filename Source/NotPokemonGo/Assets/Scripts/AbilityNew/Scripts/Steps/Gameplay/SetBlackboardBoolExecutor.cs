using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    public class SetBlackboardBoolExecutor : AbilityStepExecutor<SetBlackboardBoolStep>
    {
        public override UniTask Execute(SetBlackboardBoolStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            runtime.State.AbilityBlackboard.Set(step.Key, step.Value);
            return UniTask.CompletedTask;
        }
    }
}