using System;
using UnityEngine.Serialization;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts.Configs
{
    [Serializable]
    public class DamageStep : AbilityStepSO
    {
        [FormerlySerializedAs("damage")] public int Damage;
    }
}