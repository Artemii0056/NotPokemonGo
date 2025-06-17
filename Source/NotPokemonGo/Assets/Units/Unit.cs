using System.Collections.Generic;
using Effects;
using Stats;
using Statuses;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _statuses = new List<Status>();
        private EffectResolver _effectResolver;

        public Transform abilityPos;
        
        public void Initialize(List<StatConfig> statConfig, EffectResolver effectResolver)
        {
            _effectResolver = effectResolver;
            
            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
        }

        public float GetStat(StatType statType)
        {
            return _stats[statType].CurrentValue;
        }

        public void ReceiveDamage(EffectInfo effectInfo)
        {
            float damage = _effectResolver.CalculateFinalValue(this, effectInfo);
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