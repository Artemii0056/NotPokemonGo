using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class DamageExecutor : IAbilityStepExecutor
    {
        public UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (DamageStep)step;

            Debug.Log($"Damage {data.Damage} to {ctx.Target.name}");

            return UniTask.CompletedTask;
        }
    }
}