using System;
using AbilityNew.AbilityDefinition;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public abstract class AbilityCondition
    {
        public abstract bool Evaluate(AbilityExecutionRuntime runtime);
    }
}