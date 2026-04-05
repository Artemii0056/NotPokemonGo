using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using QteSystem;
using QteSystem.Core;
using QteSystem.Gameplay;
using QteSystem.Runtime;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class ResolveQteExecutor : AbilityStepExecutor<ResolveQteStep>
    {
        private readonly StepExecutorRegistry _registry;

        public ResolveQteExecutor(StepExecutorRegistry registry)
        {
            _registry = registry;
        }

        public override async UniTask Execute(
            ResolveQteStep step,
            AbilityExecutionRuntime runtime,
            CancellationToken ct)
        {
            var state = runtime.State;

            if (state.ActiveQte == null)
                throw new InvalidOperationException(
                    "ResolveQteStep execution failed: ActiveQte is null.");

            var activeQte = state.ActiveQte;

            float timeoutSeconds = step.TimeoutSeconds;
            
            if (state.AbilityBlackboard.TryGet<float>(BlackboardKey.ProjectileFlightTime, out var flightTime))
                timeoutSeconds = flightTime;

            try
            {
                QteResult result = await activeQte.WaitResultAsync(timeoutSeconds, ct);
                state.LastQteResult = result;

                switch (result)
                {
                    case QteResult.Fail:
                        if (step.OnFail != null)
                            await _registry.Execute(step.OnFail, runtime);
                        break;

                    case QteResult.Normal:
                        if (step.OnNormal != null)
                            await _registry.Execute(step.OnNormal, runtime);
                        break;

                    case QteResult.Perfect:
                        if (step.OnPerfect != null)
                            await _registry.Execute(step.OnPerfect, runtime);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(result), result, null);
                }
            }
            finally
            {
                activeQte.Dispose();

                if (ReferenceEquals(state.ActiveQte, activeQte))
                    state.ActiveQte = null;
            }
        }
    }
}