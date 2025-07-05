using Stats;
using Units;

namespace Effects
{
    public interface IEffectResolver
    {
        float CalculateStatModification(Unit target, StatType targetStat, EffectType effectType, float baseValue);
        void ApplyEffect(Unit target, EffectInfo effect);
    }
}