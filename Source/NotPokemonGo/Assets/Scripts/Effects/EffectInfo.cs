using Stats;

namespace Effects
{
    public struct EffectInfo
    {
        public EffectType Type;
        public StatType TargetType;
        public float Value;

        public EffectInfo(EffectType type, float value, StatType targetType)
        {
            Type = type;
            Value = value;
            TargetType = targetType;
        }
    }
}