using System;
using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Gameplay;
using Cysharp.Threading.Tasks;
using QteSystem;
using QteSystem.TestQte;
using UnityEngine;

namespace AbilityNew.Scripts.Executors.Gameplay
{
    public sealed class ResolveQteExecutor : AbilityStepExecutor<ResolveQteStep>
    {
        private readonly StepExecutorRegistry _registry;

        public ResolveQteExecutor(StepExecutorRegistry registry) => 
            _registry = registry;

        public override async UniTask Execute(ResolveQteStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            var state = runtime.State;

            if (state.ActiveQte == null)
            {
                throw new InvalidOperationException(
                    "ResolveQteStep execution failed: ActiveQte is null.");
            }
            
            var activeQte = state.ActiveQte;
            
            UniTask<QteResult> resultTask = WaitResult(activeQte);
            
            Debug.Log(step.TimeoutSeconds);
            
            UniTask timeoutTask = UniTask.Delay(
                TimeSpan.FromSeconds(step.TimeoutSeconds),
                cancellationToken: ct);

            try
            {
                var (hasQteResult, qteResult) = await UniTask.WhenAny(resultTask, timeoutTask);

                QteResult result = hasQteResult
                    ? qteResult
                    : QteResult.Fail;
                
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
            
            runtime.State.ActiveQte = null;
        }

        private static UniTask<QteResult> WaitResult(IQteSession qteSession)
        {
            var tcs = new UniTaskCompletionSource<QteResult>();

            void OnCompleted(QteResult result)
            {
                qteSession.Completed -= OnCompleted;
                tcs.TrySetResult(result);
            }

            qteSession.Completed += OnCompleted;
            return tcs.Task;
        }
    }
}