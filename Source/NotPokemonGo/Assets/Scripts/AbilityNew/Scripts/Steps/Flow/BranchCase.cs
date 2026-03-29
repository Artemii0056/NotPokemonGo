using System;
using AbilityNew.Scripts.Conditions;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Flow
{
    [Serializable]
    public class BranchCase
    {
        [SerializeReference]
        [SubclassSelector]
        public AbilityCondition Condition;

        [SerializeReference]
        [SubclassSelector]
        public AbilityStepSO Step;
    }
}