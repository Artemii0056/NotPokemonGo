using System;
using System.Collections.Generic;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using AbsolutelyNewPerfectAbilitySystem.Steps;
using Cysharp.Threading.Tasks;

namespace AbsolutelyNewPerfectAbilitySystem
{
    public class StepExecutorRegistry
    {
        private readonly Dictionary<Type, IAbilityStepExecutor> _executors;

        public StepExecutorRegistry()
        {
            _executors = new Dictionary<Type, IAbilityStepExecutor>();
        }

        public UniTask Execute(AbilityStepSO step, AbilityContext ctx)
        {
            var type = step.GetType();
            return _executors[type].Execute(step, ctx);
        }
    }
}