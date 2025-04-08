using System;
using UnityEngine;

namespace Source.StaticData.CharacterStaticData.Scripts
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField] public StatType StatsType { get; private set; }

        [Range(0, 200)] [field: SerializeField] public float Value;
    }
}