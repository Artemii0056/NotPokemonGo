using System;
using System.Collections.Generic;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Flow
{
    [Serializable]
    public class SequenceStep : AbilityStepSO
    {
        [SerializeReference] [SubclassSelector] public List<AbilityStepSO> Steps;
    }
}