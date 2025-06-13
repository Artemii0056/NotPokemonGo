using System;
using UnityEngine;

namespace Characters.Configs.Stats
{
    [Serializable]
    public class StatConfig
    {
        [field: SerializeField] public StatType StatsType { get; private set; }

        [field: SerializeField] public float Value;
    }
}