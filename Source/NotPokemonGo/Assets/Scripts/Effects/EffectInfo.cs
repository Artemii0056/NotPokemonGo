using Stats;

namespace Effects
{
    public struct EffectInfo
    {
        public EffectType Type;
        public StatType TargetType;
        public DamageType DamageType;
        public float Value;

        public EffectInfo(float value, StatType targetType, EffectType type, DamageType damageType)
        {
            Value = value;
            TargetType = targetType;
            Type = type;
            DamageType = damageType;
        }
        
        public EffectInfo(float value, StatType targetType)
        {
            Value = value;
            TargetType = targetType;
            DamageType =  DamageType.None;
            Type = EffectType.None;
        }
    }
}