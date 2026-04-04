using System;
using System.Threading;
using AbilityNew.Diagnostics;
using AbilityNew.Scripts;
using Cysharp.Threading.Tasks;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityRunner
    {
        private readonly StepExecutorRegistry _registry;
        private readonly ISignalService _signalService;
        private readonly IAbilityTraceWriter _traceWriter;

        public AbilityRunner(
            StepExecutorRegistry registry,
            ISignalService signalService,
            IAbilityTraceWriter traceWriter)
        {
            _registry = registry;
            _signalService = signalService;
            _traceWriter = traceWriter;
        }

        public async UniTask<AbilityExecutionResult> RunAbility(
            AbilitySo ability,
            AbilityExecutionContext context,
            CancellationToken cancellationToken = default)
        {
            _signalService.Reset();

            if (ability == null)
                throw new ArgumentNullException(nameof(ability));

            var runtime = new AbilityExecutionRuntime(
                ability,
                context,
                cancellationToken,
                _traceWriter);

            runtime.TraceInfo(
                AbilityTraceSource.Runtime,
                "Ability",
                nameof(AbilityRunner),
                "Execution started");

            try
            {
                for (int i = 0; i < ability.Steps.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var step = ability.Steps[i];
                    string stepName = step?.GetType().Name ?? "NullStep";

                    runtime.TraceInfo(
                        AbilityTraceSource.Runtime,
                        "Ability",
                        nameof(AbilityRunner),
                        $"StepIndex={i}, Step={stepName}, Phase=Start");

                    if (runtime.State.IsInterrupted)
                    {
                        runtime.Result.MarkInterrupted();

                        runtime.TraceInfo(
                            AbilityTraceSource.Runtime,
                            "Ability",
                            nameof(AbilityRunner),
                            "Execution interrupted");

                        return runtime.Result;
                    }

                    if (runtime.State.IsCancelled)
                    {
                        runtime.Result.MarkCancelled();

                        runtime.TraceInfo(
                            AbilityTraceSource.Runtime,
                            "Ability",
                            nameof(AbilityRunner),
                            "Execution cancelled");

                        return runtime.Result;
                    }

                    await _registry.Execute(step, runtime);

                    runtime.TraceInfo(
                        AbilityTraceSource.Runtime,
                        "Ability",
                        nameof(AbilityRunner),
                        $"StepIndex={i}, Step={stepName}, Phase=End");
                }

                runtime.Result.MarkCompleted();

                runtime.TraceInfo(
                    AbilityTraceSource.Runtime,
                    "Ability",
                    nameof(AbilityRunner),
                    "Execution finished successfully");

                return runtime.Result;
            }
            catch (OperationCanceledException)
            {
                runtime.Result.MarkCancelled();

                runtime.TraceInfo(
                    AbilityTraceSource.Runtime,
                    "Ability",
                    nameof(AbilityRunner),
                    "Execution cancelled by token");

                throw;
            }
            catch (Exception ex)
            {
                runtime.TraceError("Ability", nameof(AbilityRunner), ex);
                throw;
            }
        }
    }
}