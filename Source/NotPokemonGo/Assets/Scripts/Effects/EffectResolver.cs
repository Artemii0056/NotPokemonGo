using System;
using Stats;
using Units;

namespace Effects
{
    public class EffectResolver : IEffectResolver
    {
        public float CalculateFinalValue(Unit target, EffectInfo effect)
        {
            float value = effect.Value;

            switch (effect.Type)
            {
                case EffectType.ModifyStat:
                    value = CalculateStatModification(target, effect.TargetType, effect.Value);
                    break;

                default:
                    throw new NotSupportedException($"EffectType '{effect.Type}' не поддерживается.");
            }

            return value;
        }
        
        public void ApplyEffect(Unit target, EffectInfo effect)
        {
            float finalValue = CalculateFinalValue(target, effect);
            target.ChangeStatValue(effect.TargetType, finalValue);
        }

        private float CalculateStatModification(Unit target, StatType targetStat, float baseValue)
        {
            float finalValue = baseValue;
            
            switch (targetStat)
            {
                case StatType.Health:
                    finalValue = -finalValue;
                    
                    // if (baseValue < 0)
                    // {
                    //     // Damage: учитывать броню
                    //     float armor = target.GetStat(StatType.ArmorChance);
                    //     finalValue = -Math.Min(0, baseValue); //TODO Добавить броню!!!
                    //     
                    //     Debug.Log(finalValue);
                    // }
                    break;

                case StatType.AgilityRestoreSpeed:
                    break;

                case StatType.ArmorChance:
                    break;

                case StatType.Mana:
                    break;
            }

            return finalValue;
        }
    }
}