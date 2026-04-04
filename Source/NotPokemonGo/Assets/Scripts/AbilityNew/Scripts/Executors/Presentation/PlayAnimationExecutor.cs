using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Diagnostics;
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
            const string ExecutorName = nameof(PlayAnimationExecutor);
            const string StepName = nameof(PlayAnimationStep);

            runtime.TraceStepEnter(StepName, ExecutorName);

            try
            {
                var source = runtime.Context.Source;
                var animatorController = source.AnimatorController;
                var animationName = step.Animation.name;
                int hash = Animator.StringToHash(animationName);

                runtime.TraceInfo(AbilityTraceSource.Executor, StepName, ExecutorName,
                    $"Animation={animationName}, Hash={hash}");

                animatorController.Play(hash);

                runtime.TraceStepExit(StepName, ExecutorName, $"Played animation {animationName}");
                return UniTask.CompletedTask;
            }
            catch (Exception ex)
            {
                runtime.TraceError(StepName, ExecutorName, ex);
                throw;
            }
        }
    }
}