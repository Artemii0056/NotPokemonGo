using System;
using AbilityNew.AbilityDefinition;
using UnityEngine;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public class NotCondition : AbilityCondition
    {
        [SerializeReference]
        public AbilityCondition Condition;

        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            if (Condition == null)
                return false;

            return !Condition.Evaluate(runtime);
        }
    }
}