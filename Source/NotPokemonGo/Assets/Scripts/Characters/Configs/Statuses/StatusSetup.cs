using System;
using Characters.Configs.Effects;
using UnityEngine;

namespace Characters.Configs.Statuses
{
    [Serializable]
    public class StatusSetup
    {
        [field: SerializeField] public EffectSetup EffectSetup { get; private set; }

        [field: SerializeField] [Range(1, 10)]  public float Duration;
    }
}