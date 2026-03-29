using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.Configs;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.AbilityExecutor
{
    public abstract class AbilityStepExecutor<TStep> : IAbilityStepExecutor<TStep>
        where TStep : AbilityStepSO
    {
        public Type StepType => typeof(TStep);

        public async UniTask Execute(AbilityStepSO step, AbilityExecutionRuntime runtime)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            if (runtime == null)
                throw new ArgumentNullException(nameof(runtime));
            
            if (step is not TStep typedStep)
            {
                throw new InvalidOperationException(
                    $"Invalid step type. Expected {typeof(TStep).Name}, got {step.GetType().Name}");
            }

            await Execute(typedStep, runtime, runtime.CancellationToken);
        }

        public abstract UniTask Execute(TStep step, AbilityExecutionRuntime runtime,  CancellationToken ct);
    }
}