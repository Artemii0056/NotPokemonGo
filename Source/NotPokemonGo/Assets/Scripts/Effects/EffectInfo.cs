using Stats;

namespace Effects
{
    public struct EffectInfo
    {
        public EffectType Type;
        public StatType TargetType;
        public float Value;

        public EffectInfo(float value, StatType targetType, EffectType type)
        {
            Value = value;
            TargetType = targetType;
            Type = type;
        }
    }
}