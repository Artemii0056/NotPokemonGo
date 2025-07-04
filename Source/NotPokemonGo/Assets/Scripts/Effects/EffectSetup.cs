using System;
using Characters.Configs.Stats;
using StatType = Stats.StatType;

namespace Effects
{
    [Serializable]
    public class EffectSetup
    {
        public EffectType Type;
        public StatType TargetType;

        public float Value;
    }
}