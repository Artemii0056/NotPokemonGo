using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Presentation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Presentation
{
    public class PlayAnimationExecutor : AbilityStepExecutor<PlayAnimationStep>
    {
        public override UniTask Execute(PlayAnimationStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            runtime.Context.Source.AnimatorController.Play(Animator.StringToHash(step.Animation.name));

            return UniTask.CompletedTask;
        }
    }
}