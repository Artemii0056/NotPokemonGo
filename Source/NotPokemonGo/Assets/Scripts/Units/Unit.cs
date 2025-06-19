using System.Collections.Generic;
using System.Linq;
using Abilities.MV;
using DefaultNamespace;
using Effects;
using Stats;
using Statuses;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();
        private EffectResolver _effectResolver;
        
        private List<AbilityModel> _abilityModels = new List<AbilityModel>();

        public PlatoonType PlatoonType;

        public List<AbilityModel> AbilityModels => _abilityModels.ToList();


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
            _imposedStatuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            _imposedStatuses.Remove(status);
        }

        public void AddAbility(AbilityModel ability) => 
            _abilityModels.Add(ability);

        public void Tick(float deltaTime)
        {
            if (_abilityModels.Count > 0)
            {
                foreach (var model in _abilityModels) 
                    model.UpdateTime(deltaTime);
            }
        }
    }
}