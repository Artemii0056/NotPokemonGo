using System;
using AbilityNew.Scripts;
using Cysharp.Threading.Tasks;

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
            AbilityExecutionContext context)
        {
            if (ability == null)
                throw new ArgumentNullException(nameof(ability));

            var runtime = new AbilityExecutionRuntime(context);

            foreach (var step in ability.Steps)
            {
                if (runtime.State.IsInterrupted)
                {
                    runtime.Result.MarkInterrupted();
                    return runtime.Result;
                }

                if (runtime.State.IsCancelled)
                {
                    runtime.Result.MarkCancelled();
                    return runtime.Result;
                }

                await _registry.Execute(step, runtime);
            }

            runtime.Result.MarkCompleted();
            return runtime.Result;
        }
    }
}