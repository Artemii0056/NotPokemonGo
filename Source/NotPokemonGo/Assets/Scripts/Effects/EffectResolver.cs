using System;
using Abilities;
using Abilities.MV;
using Infrastructure;
using Stats;
using Units;
using UnityEngine;

namespace Effects
{
    public class EffectResolver : IEffectResolver
    {
        private readonly ISourceProvider _sourceProvider;
        private readonly IAbilityService _abilityService;

        public EffectResolver(
            ISourceProvider sourceProvider,
            IAbilityService abilityService)
        {
            _sourceProvider = sourceProvider;
            _abilityService = abilityService;
        }

        public void ApplyEffect(Unit target, EffectInfo effect) //Сейчас полная фигня
        {
            if (effect.Type == EffectType.Damage && SearchCounterAttackAbility(target, out AbilityModel abilityModel))
            {
                _abilityService.HandleCounterAttack(target, abilityModel);
                return;
            }

            float finalValue = CalculateStatModification(target, effect.TargetType, effect.Type, effect.Value); 
            target.ChangeStatValue(effect.TargetType, finalValue);
        }

        private bool SearchCounterAttackAbility(Unit target, out AbilityModel ability)
        {
            ability = null;

            foreach (var abilityModel in target.AbilityModels)
            {
                if (abilityModel.AbilityType == AbilityType.CounterAttack && abilityModel.IsReady())
                {
                    ability = abilityModel;
                    return true;
                }
            }

            return false;
        }

        private float CalculateStatModification(
            Unit target,
            StatType targetStat,
            EffectType effectType,
            float baseValue)
        {
            float qteModificator = _sourceProvider.Source.GetStat(StatType.QteDamageModifier);

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
                                target.UnitAnimatorController.Play(Constants.BaseAnimations.TakeDamage);
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