using System;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class AbilityStatSetup
    {
        [field: SerializeField] public AbilityStatType StatsType { get; private set; }

        public float Value;
    }
}