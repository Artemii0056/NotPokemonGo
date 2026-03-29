using System;
using AbilityNew.AbilityDefinition;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public class BlackboardBoolCondition : AbilityCondition
    {
        public BlackboardKey Key;
        public bool ExpectedValue = true;

        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            return runtime.State.AbilityBlackboard.TryGet<bool>(Key, out var value) &&
                   value == ExpectedValue;
        }
    }
}