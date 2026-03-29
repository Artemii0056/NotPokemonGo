using System;
using System.IO;
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
        public override UniTask Execute(
            PlayAnimationStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            Trace("PlayAnimationExecutor START");

            var source = runtime.Context.Source;
            Trace($"source: {source}");

            var animatorController = source.AnimatorController;
            Trace($"animatorController: {animatorController}");

            var animation = step.Animation;
            Trace($"animation ref: {animation}");

            string animationName = animation.name;
            Trace($"animation name: {animationName}");

            int hash = Animator.StringToHash(animationName);
            Trace($"animation hash: {hash}");

            Trace("Before AnimatorController.Play");
            animatorController.Play(hash);
            Trace("After AnimatorController.Play");

            Trace("PlayAnimationExecutor END");

            return UniTask.CompletedTask;
        }
        
        private void Trace(string message)
        {
            var path = Path.Combine(Application.persistentDataPath, "ability_trace.log");
            File.AppendAllText(path, $"{DateTime.Now:HH:mm:ss.fff} | {message}\n");
        }
    }
}