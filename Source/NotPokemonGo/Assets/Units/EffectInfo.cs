using Effects;

namespace Units
{
    public struct EffectInfo
    {
        public EffectType Type;
        public float Value;

        public EffectInfo(EffectType type, float value)
        {
            Type = type;
            Value = value;
        }
    }
}