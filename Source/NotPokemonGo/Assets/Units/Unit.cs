using System.Collections.Generic;
using Characters.Configs.Stats;
using Characters.Configs.Statuses;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _statuses = new List<Status>();
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

        public void AddStatusEffect(Status status)
        {
            _statuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            _statuses.Remove(status);
        }

        public void UpdateStatuses()
        {
            if (_statuses.Count <= 0)
                return;
            
            foreach (var status in _statuses)
            {
                status.Tick();
            }
        }
    }
}