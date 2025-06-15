using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class StatConfig
    {
        [field: SerializeField] public StatType StatsType { get; private set; }

        [field: SerializeField] public float Value;
    }
}