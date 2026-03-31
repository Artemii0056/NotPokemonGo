using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Results;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;
using Units;

namespace AbilityNew.Scripts.Executors.Flow
{
    public sealed class RequestCounterAttackExecutor : AbilityStepExecutor<RequestCounterAttackStep>
    {
        public override UniTask Execute(
            RequestCounterAttackStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            Unit reactor = runtime.Context.Target;
            Unit attacker = runtime.Context.Source;

            if (reactor != null && attacker != null)
                runtime.Result.AddCounterAttackRequest(new CounterAttackRequest(reactor, attacker));

            return UniTask.CompletedTask;
        }
    }
}