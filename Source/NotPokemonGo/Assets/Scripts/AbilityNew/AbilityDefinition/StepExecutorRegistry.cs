using System;
using System.Collections.Generic;
using System.Linq;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Configs;
using Cysharp.Threading.Tasks;

namespace AbilityNew.AbilityDefinition
{
    public sealed class StepExecutorRegistry
    {
        private readonly Dictionary<Type, IAbilityStepExecutor> _executors;

        public StepExecutorRegistry(IEnumerable<IAbilityStepExecutor> executors)
        {
            if (executors == null)
                throw new ArgumentNullException(nameof(executors));
            
            _executors = executors.ToDictionary(x => x.StepType, x => x);
        }

        public UniTask Execute(AbilityStepSO step, AbilityExecutionRuntime runtime)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            if (runtime == null)
                throw new ArgumentNullException(nameof(runtime));

            Type stepType = step.GetType();

            if (_executors.TryGetValue(stepType, out IAbilityStepExecutor executor) == false)
            {
                throw new InvalidOperationException(
                    $"Executor for step type '{stepType.Name}' is not registered.");
            }

            return executor.Execute(step, runtime);
        }

        public void AddExecutor(IAbilityStepExecutor executor)
        {
            _executors[executor.StepType] = executor;
        }

        public bool HasExecutor(Type stepType) => 
            _executors.ContainsKey(stepType);
    }
}