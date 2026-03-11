using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class WaitSignalExecutor : IAbilityStepExecutor
    {
        private readonly SignalService _signals;

        public WaitSignalExecutor(SignalService signals)
        {
            _signals = signals;
        }

        public UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (WaitSignalStep)step;
            
            Debug.Log("WaitSignalExecutor");
            return _signals.Wait(data.Signal);
        }
    }
}