using Effects;

namespace Units
{
    public struct EffectInfo
    {
        public EffectType Type;
        public float Value;
        public Unit Source;

        public EffectInfo(EffectType type, float value, Unit source)
        {
            Type = type;
            Value = value;
            Source = source;
        }
    }
}