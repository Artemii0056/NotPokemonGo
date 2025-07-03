using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.MV;
using Characters;
using Characters.Configs;
using Effects;
using Stats;
using Statuses;
using Units.AnimationControllers;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [field: SerializeField] public UnitAnimatorController UnitAnimatorController { get; private set; }

        [SerializeField] private List<AbilityAnchor> abilityAnchors;

        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();
        private IEffectResolver _effectResolver;
        private UnitAnimatorTrigger _unitAnimatorTrigger;

        private List<AbilityModel> _abilityModels = new List<AbilityModel>();

        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;

        public event Action<Unit> Prepared; 
        public event Action AnimationActionEnded; 
        
        public PlatoonType PlatoonType { get; private set; }
        public UnitStep Step { get; private set; }
        
        [field: SerializeField] public UnitType UnitType { get; private set; }

        public List<Status> ImposedStatuses => _imposedStatuses.ToList();
        public List<AbilityModel> AbilityModels => _abilityModels.ToList();
        public List<AbilityAnchor> AbilityAnchors => abilityAnchors.ToList();

        public Transform abilityPos;

        public void Construct(
            List<StatConfig> statConfig,
            IEffectResolver effectResolver,
            PlatoonType platoonType, 
            UnitAnimatorTrigger unitAnimatorTrigger, 
            UnitStep step)
        {
            _unitAnimatorTrigger = unitAnimatorTrigger;
            _effectResolver = effectResolver;
            PlatoonType = platoonType;

           Step = step; 

            foreach (var statSetup in statConfig)
            {
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));
            }
            
             _unitAnimatorTrigger.ActionEnded += OnActionEnded;
            Step.ActionEnded += OnActionEnded;
        }

        private void OnDestroy()
        {
            _unitAnimatorTrigger.ActionEnded -= OnActionEnded;
            Step.ActionEnded -= OnActionEnded;
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

        public void AddStatus(Status status)
        {
            StatusAdded?.Invoke(status);
            _imposedStatuses.Add(status);
        }

        public void RemoveStatus(Status status)
        {
            _imposedStatuses.Remove(status);
            StatusRemoved?.Invoke(status);
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
            
            Prepared?.Invoke(this);
        }

        private void OnActionEnded() => 
            AnimationActionEnded?.Invoke();
    }
}