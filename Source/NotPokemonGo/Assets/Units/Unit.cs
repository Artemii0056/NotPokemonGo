using System.Collections.Generic;
using Characters.Configs.Stats;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private DamageResolver _damageResolver;
        
        public void Initialize(List<StatConfig> statConfig, DamageResolver damageResolver)
        {
            _damageResolver = damageResolver;
            
            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
        }

        public float GetStat(StatType statType)
        {
            return _stats[statType].CurrentValue;
        }

        public void ReceiveDamage(DamageInfo damageInfo)
        {
            float damage = _damageResolver.CalculateFinalDamage(this, damageInfo);
            ChangeValue(StatType.Health, damage);
        }

        public void ChangeValue(StatType statType, float value)
        {
            _stats[statType].Modify(value);
        }
    }
}