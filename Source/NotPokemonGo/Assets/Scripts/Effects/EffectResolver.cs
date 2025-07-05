using System;
using Stats;
using Units;

namespace Effects
{
    public class EffectResolver : IEffectResolver
    {
        public void ApplyEffect(Unit target, EffectInfo effect)
        {
            float finalValue = CalculateStatModification(target, effect.TargetType,effect.Type, effect.Value);
            target.ChangeStatValue(effect.TargetType, finalValue);
        }

        public float CalculateStatModification(Unit target, StatType targetStat,EffectType effectType, float baseValue)
        {
            float finalValue = baseValue;
            
            switch (targetStat)
            {
                case StatType.Health:

                    switch (effectType)
                    {
                        case EffectType.Damage:
                            finalValue = -finalValue;
                            break;
                        
                        case EffectType.Heal:
                            break;
                        
                        default:
                            throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null);
                    }
                    
                    
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