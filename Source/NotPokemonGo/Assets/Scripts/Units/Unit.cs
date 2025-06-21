using System.Collections.Generic;
using Effects;
using Stats;
using Statuses;
using UnityEngine;
using VContainer;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();
        private IEffectResolver _effectResolver;

        public Transform abilityPos;

        public void Construct(List<StatConfig> statConfig)
        {
            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
        }
        
        [Inject]
        public void Initialize(IEffectResolver effectResolver) => 
            _effectResolver = effectResolver;

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
            _imposedStatuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            _imposedStatuses.Remove(status);
        }
    }
}