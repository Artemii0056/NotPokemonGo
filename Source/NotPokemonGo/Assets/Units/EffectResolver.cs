using System;
using Effects;
using Stats;

namespace Units
{
    public class EffectResolver
    {
        public float CalculateFinalValue(Unit target, EffectInfo effectInfo)
        {
            float value = effectInfo.Value;

            switch (effectInfo.Type)
            {
                case EffectType.Damage:
                    float armor = target.GetStat(StatType.ArmorChance);
                    value = -Math.Max(0, effectInfo.Value - armor);
                    break;

                case EffectType.Heal:
                    value = effectInfo.Value;
                    break;
            }

            return value;
        }
    }
}