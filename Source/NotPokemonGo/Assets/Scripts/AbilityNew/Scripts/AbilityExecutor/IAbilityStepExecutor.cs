using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.Configs;
using Cysharp.Threading.Tasks;

namespace AbilityNew.Scripts.AbilityExecutor
{
    public interface IAbilityStepExecutor
    {
        Type StepType { get; }
        UniTask Execute(AbilityStepSO step, AbilityExecutionRuntime runtime);
    }

    public interface IAbilityStepExecutor<in TStep> : IAbilityStepExecutor
        where TStep : AbilityStepSO
    {
        UniTask Execute(TStep step, AbilityExecutionRuntime runtime,  CancellationToken ct);
    }
}