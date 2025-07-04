using System;
using Stats;
using Units;
using UnityEngine;

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

        private float CalculateStatModification(Unit target, StatType targetStat, float baseValue)
        {
            float finalValue = baseValue;
            
            Debug.Log("CalculateStatModification");

            switch (targetStat)
            {
                case StatType.Health:
                    Debug.Log("StatType.Health" + finalValue);

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
                    // Например, любые изменения скорости не модифицируются, возвращаем как есть
                    break;

                case StatType.ArmorChance:
                    // Можно добавить модификаторы (бафы/дебафы) или ограничения
                    break;

                case StatType.Mana:
                    // Здесь можно ограничить max/min значения или проверять флаги
                    break;

                default:
                    // Поддержка новых статов
                    break;
            }

            return finalValue;
        }

        public void ApplyEffect(Unit target, EffectInfo effect)
        {
            float finalValue = CalculateFinalValue(target, effect);
            target.ChangeStatValue(effect.TargetType, finalValue);
        }
        
        // public float CalculateFinalValue(Unit target, EffectInfo effectInfo)
        // {
        //     float value = effectInfo.Value;
        //
        //     switch (effectInfo.Type)
        //     {
        //         case EffectType.Damage:
        //             float armor = target.GetStat(StatType.ArmorChance);
        //             value = -Math.Max(0, effectInfo.Value - armor);
        //             break;
        //
        //         case EffectType.Heal:
        //             value = effectInfo.Value;
        //             break;
        //     }
        //
        //     return value;
        // }
    }
}