using System;
using AbilityNew.AbilityDefinition;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public class TargetExistsCondition : AbilityCondition
    {
        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            return runtime.Context.Target != null;
        }
    }
}