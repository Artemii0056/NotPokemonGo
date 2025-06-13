using System;
using UnityEngine;

namespace Characters.Configs.Effects
{
    [Serializable]
    public class EffectSetup
    {
        [field: SerializeField] public EffectType EffectType { get; private set; }

        [field: SerializeField] [Range(0, 200)]  public float Value;
    }
}