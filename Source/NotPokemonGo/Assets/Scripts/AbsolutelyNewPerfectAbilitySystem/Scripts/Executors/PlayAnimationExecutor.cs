using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Executors
{
    public class PlayAnimationExecutor : IAbilityStepExecutor
    {
        public UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var data = (PlayAnimationStep)step;

            ctx.Source.AnimatorController.Play(Animator.StringToHash(data.Animation.name));

            return UniTask.CompletedTask;
        }
    }
}