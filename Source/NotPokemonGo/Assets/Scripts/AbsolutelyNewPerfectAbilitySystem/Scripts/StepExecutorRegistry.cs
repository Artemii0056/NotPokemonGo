using System;
using System.Collections.Generic;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Executors;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts
{
    public class StepExecutorRegistry
    {
        private readonly Dictionary<Type, IAbilityStepExecutor> _executors;

        public StepExecutorRegistry(Dictionary<Type, IAbilityStepExecutor> executors)
        {
            _executors = executors;
        }

        public UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var type = step.GetType();
            return _executors[type].Execute(step, ctx);
        }

        public void AddExecutor(Type type, IAbilityStepExecutor executor)
        {
            _executors[type] = executor;
        }
    }
}