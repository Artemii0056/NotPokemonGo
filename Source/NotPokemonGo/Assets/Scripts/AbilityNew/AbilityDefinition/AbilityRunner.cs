using System;
using System.IO;
using System.Threading;
using AbilityNew.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityRunner
    {
        private readonly StepExecutorRegistry _registry;

        public AbilityRunner(StepExecutorRegistry registry)
        {
            _registry = registry;
        }

        public async UniTask<AbilityExecutionResult> RunAbility(
            AbilitySO ability,
            AbilityExecutionContext context,
            CancellationToken cancellationToken = default)
        {
            Trace("AbilityRunner.RunAbility START");

            if (ability == null)
                throw new ArgumentNullException(nameof(ability));

            var runtime = new AbilityExecutionRuntime(context, cancellationToken);
            Trace("AbilityExecutionRuntime created");

            for (int i = 0; i < ability.Steps.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                var step = ability.Steps[i];
                Trace($"Step #{i} START: {step?.GetType().Name}");

                if (runtime.State.IsInterrupted)
                {
                    Trace("Interrupted");
                    runtime.Result.MarkInterrupted();
                    return runtime.Result;
                }

                if (runtime.State.IsCancelled)
                {
                    Trace("Cancelled");
                    runtime.Result.MarkCancelled();
                    return runtime.Result;
                }

                await _registry.Execute(step, runtime);

                Trace($"Step #{i} END: {step?.GetType().Name}");
            }

            runtime.Result.MarkCompleted();
            Trace("AbilityRunner.RunAbility END");
            return runtime.Result;
        }
        
        private void Trace(string message)
        {
            var path = Path.Combine(Application.persistentDataPath, "ability_trace.log");
            File.AppendAllText(path, $"{DateTime.Now:HH:mm:ss.fff} | {message}\n");
        }
    }
}