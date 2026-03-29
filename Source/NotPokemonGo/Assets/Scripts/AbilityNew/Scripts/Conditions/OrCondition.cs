using System;
using System.Collections.Generic;
using AbilityNew.AbilityDefinition;
using UnityEngine;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public class OrCondition : AbilityCondition
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
                    continue;

                if (condition.Evaluate(runtime))
                    return true;
            }

            return false;
        }
    }
}