using System;
using Stats;
using Units;
using UnityEngine;

namespace Effects
{
    public class EffectResolver : IEffectResolver
    {
        public void ApplyEffect(Unit source, Unit target, EffectInfo effect) 
        {
            float finalValue = CalculateStatModification(source,target, effect.TargetType, effect.Type, effect.Value); 
            target.ChangeStatValue(finalValue, effect.TargetType);
        }

        public void ApplyEffect(Unit target, EffectInfo effect) => 
            target.ChangeStatValue(effect.Value, effect.TargetType);

        private float CalculateStatModification(
            Unit source,
            Unit target,
            StatType targetStat,
            EffectType effectType,
            float baseValue)
        {
            float qteModificator = source.GetStat(StatType.QteDamageModifier);

            float finalValue;

            if (qteModificator > 0)
            {
                finalValue = baseValue * (qteModificator + 1);
            }
            else
            {
                finalValue = baseValue;
            }

            switch (targetStat)
            {
                case StatType.Health:
                    switch (effectType)
                    {
                        case EffectType.Damage:
                            if (target.IsAlive)
                            {
                                finalValue = -finalValue;
                               // target.UnitAnimatorController.Play(Constants.BaseAnimations.TakeDamage);
                            }
                            else
                            {
                                finalValue = 0;
                            }

                            // Debug.Log(finalValue);
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