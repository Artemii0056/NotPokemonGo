using System;
using System.Collections.Generic;
using AbilityNew.AbilityDefinition;
using UnityEngine;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public class AndCondition : AbilityCondition
    {
        [SerializeReference]
        public List<AbilityCondition> Conditions = new();

        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            if (Conditions == null || Conditions.Count == 0)
                return false;

            foreach (var condition in Conditions)
            {
                if (condition == null)
                    return false;

                if (!condition.Evaluate(runtime))
                    return false;
            }

            return true;
        }
    }
}