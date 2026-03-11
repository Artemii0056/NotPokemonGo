using System;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Flow
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