using System;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Gameplay
{
    [Serializable]
    public class ResolveQteStep : AbilityStepSO
    {
        [SerializeReference, SubclassSelector] public AbilityStepSO OnFail;
        [SerializeReference, SubclassSelector]  public AbilityStepSO OnNormal;
        [SerializeReference, SubclassSelector]  public AbilityStepSO OnPerfect;
    }
}