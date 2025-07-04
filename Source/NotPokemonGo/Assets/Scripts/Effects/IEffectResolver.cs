using Units;

namespace Effects
{
    public interface IEffectResolver
    {
        float CalculateFinalValue(Unit target, EffectInfo effectInfo);
        void ApplyEffect(Unit target, EffectInfo effect);
    }
}