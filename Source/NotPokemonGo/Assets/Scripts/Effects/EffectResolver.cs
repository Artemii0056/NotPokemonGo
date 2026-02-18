using System;
using Stats;
using Units;
using UnityEngine;

namespace Effects
{
    public class EffectResolver : IEffectResolver
    {
        public event Action<EffectDataPayload> EffectApplied;

        public void ApplyEffect(Unit target, EffectInfo effect)
        {
            float delta = CalculateStatModification(target, effect.TargetType, effect.Type, effect.Value);
            
            float newValue = target.GetStat(effect.TargetType) + delta;
            
            target.ChangeStatValue(newValue, effect.TargetType);

            EffectApplied?.Invoke(new EffectDataPayload(target, delta, effect));
        }
        
        private float CalculateStatModification(
            Unit target,
            StatType targetStat,
            EffectType effectType,
            float value)
        {
            float finalValue = 0;

            if (target.IsAlive == false)
                return 0;

            switch (targetStat)
            {
                case StatType.Health:
                    switch (effectType)
                    {
                        case EffectType.Damage:
                            return -Mathf.Abs(value);

                        case EffectType.Heal:
                            return Mathf.Abs(value);

                        default:
                            Debug.Log(effectType);
                            throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null);
                    }
            }

            return finalValue;
        }
        
    }
}