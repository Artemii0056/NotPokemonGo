using Units;

namespace Effects
{
    public struct EffectDataPayload
    {
        public EffectDataPayload(Unit target, float finalValue, EffectInfo effect)
        {
            Target = target;
            FinalValue = finalValue;
            Effect = effect;
        }
            
        public Unit Target { get; }
        public float FinalValue { get; }
        public EffectInfo Effect { get; }
    }
}