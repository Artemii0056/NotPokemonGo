using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps
{
    [Serializable]
    public class SequenceStep : AbilityStepSO
    {
        [SerializeReference] [SubclassSelector] public List<AbilityStepSO> Steps;
    }
}