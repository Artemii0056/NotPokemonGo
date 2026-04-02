using System;
using System.Collections.Generic;

namespace AbilityNew.Scripts.Presentation
{
    namespace AbilityNew.Scripts.Presentation
    {
        public sealed class PresentationStepExecutorRegistry
        {
            private readonly Dictionary<Type, IPresentationStepExecutor> _executors = new();

            public PresentationStepExecutorRegistry(IEnumerable<IPresentationStepExecutor> executors)
            {
                foreach (var executor in executors)
                    _executors[executor.StepType] = executor;
            }

            public void Execute(PresentationStep step, AbilityPresentationContext context)
            {
                if (step == null)
                    return;

                var stepType = step.GetType();

                if (_executors.TryGetValue(stepType, out var executor) == false)
                    throw new InvalidOperationException($"Presentation executor not found for step type: {stepType.Name}");

                executor.Execute(step, context);
            }
        }
    }
}