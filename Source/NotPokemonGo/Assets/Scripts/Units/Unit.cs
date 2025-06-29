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
using UnityEngine.Serialization;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private List<AbilityAnchor> abilityAnchors;

        [FormerlySerializedAs("AbilityAnimationControllerBase")]
        public UnitAnimatorController unitAnimatorController;

        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();
        private IEffectResolver _effectResolver;

        private List<AbilityModel> _abilityModels = new List<AbilityModel>();
        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;

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
            PlatoonType platoonType
        )
        {
            _effectResolver = effectResolver;
            PlatoonType = platoonType;

            Step = new UnitStep(5);

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
            Debug.Log(_stats[statType].CurrentValue + ", " + value);
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

            Step.IncreaseCurrentValue(deltaTime * GetStat(StatType.Agility));
        }
    }
}