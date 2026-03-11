using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Flow;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Flow
{
    public class WaitSignalExecutor : AbilityStepExecutor<WaitSignalStep>
    {
        private readonly SignalService _signals;

        public WaitSignalExecutor(SignalService signals) => 
            _signals = signals;


        public override UniTask Execute(WaitSignalStep step, AbilityExecutionRuntime runtime)
        {
            Debug.Log("WaitSignalExecutor");
            return _signals.Wait(step.Signal);
        }
    }
}