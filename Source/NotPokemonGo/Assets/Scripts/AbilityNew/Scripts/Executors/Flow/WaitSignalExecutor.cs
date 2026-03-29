using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class WaitSignalExecutor : AbilityStepExecutor<WaitSignalStep>
    {
        private readonly ISignalService _signals;

        public WaitSignalExecutor(ISignalService signals) => 
            _signals = signals ?? throw new ArgumentNullException(nameof(signals));

        public override async  UniTask Execute(WaitSignalStep step, AbilityExecutionRuntime runtime, CancellationToken ct) => 
            await _signals.WaitAsync(step.Signal, ct);
    }
}