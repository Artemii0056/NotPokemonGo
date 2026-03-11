using System;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class WaitingExecutor : IAbilityStepExecutor
    {
        public async UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (WaitingStep)step;

            await UniTask.Delay(TimeSpan.FromSeconds(data.Duration));
        }
    }
}