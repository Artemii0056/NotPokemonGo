using System;
using Stats;

namespace Effects
{
    [Serializable]
    public class EffectSetup
    {
        public EffectType Type;
        public StatType TargetType;
        public DamageType DamageType;

        public float Value;
    }
}