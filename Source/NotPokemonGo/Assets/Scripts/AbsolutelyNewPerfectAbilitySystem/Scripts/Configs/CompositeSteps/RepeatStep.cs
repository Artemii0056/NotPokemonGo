using System;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps
{
    [Serializable]
    public class RepeatStep : AbilityStepSO
    {
        public int Count;
        
        [SerializeReference] 
        [SubclassSelector] 
        public AbilityStepSO Step;
    }
}