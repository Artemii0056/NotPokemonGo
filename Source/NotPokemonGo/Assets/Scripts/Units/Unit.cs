using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.MV;
using Assets;
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
        public Transform abilityPos;
        [field: SerializeField] public UnitAnimatorController UnitAnimatorController { get; private set; }

        [SerializeField] private List<AbilityAnchor> abilityAnchors;

        [field: SerializeField] public UnitType UnitType { get; private set; }


        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();
        private List<Status> _imposedStatuses = new List<Status>();

        private List<AbilityModel> _abilityModels = new List<AbilityModel>();
        
        public PlatoonType PlatoonType { get; private set; }
        public UnitStep Step { get; private set; }

        public List<Status> ImposedStatuses => _imposedStatuses.ToList();
        public List<AbilityModel> AbilityModels => _abilityModels.ToList();
        public List<AbilityAnchor> AbilityAnchors => abilityAnchors.ToList();
        public event Action Ticked;

        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;

        public event Action<Unit> Prepared; 
        public event Action<float, float> AgilityChanged;
        public event Action<float, float> HealthChanged;

        public void Construct(
            List<StatConfig> statConfig,
            UnitStep step,
            PlatoonType platoonType)
        {
            PlatoonType = platoonType;

            Step = step; 

            foreach (var statSetup in statConfig) 
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));

            foreach (StatSetup stat in _stats.Values) 
                stat.CurrentValueChanged += StatChanged;
            
            HealthChanged?.Invoke(GetStat(StatType.Health), GetStat(StatType.MaxHealth));
        }

        private void OnDestroy()
        {
            foreach (StatSetup stat in _stats.Values) 
                stat.CurrentValueChanged -= StatChanged;
        }
        
        private void StatChanged(float current, StatType statType)
        {
            switch (statType)
            {
                case StatType.Health:
                    float currentHealth = GetStat(StatType.Health);

                    if (currentHealth <= 0)
                    {
                        // Шото там с анимациями и слайдерами
                    }
                    else
                    {
                        var maxHealth = GetStat(StatType.MaxHealth);
                        HealthChanged?.Invoke(currentHealth, maxHealth);
                    }

                    break;
                case StatType.Mana:
                    break;
                case StatType.DodgeChance:
                    break;
                case StatType.Accuracy:
                    break;
                case StatType.ArmorChance:
                    break;
                case StatType.Damage:
                    break;
                case StatType.CurrentAgility:
                    AgilityChanged?.Invoke(GetStat(StatType.CurrentAgility), GetStat(StatType.MaxAgility));
                    break;
                case StatType.MaxAgility:
                    break;
                case StatType.AgilityRestoreSpeed:
                    break;
            }
        }

        public float GetStat(StatType statType)
        {
            return _stats[statType].CurrentValue;
        }

        public void ChangeStatValue(StatType statType, float value)
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
        
        public void ResetAgility() =>
            _stats[StatType.CurrentAgility].Set(0);

        public void Tick()
        {
            TickAbilities();

            TickAgility();
            
            Ticked?.Invoke();
        }

        private void TickAbilities()
        {
            if (_abilityModels.Count > 0)
            {
                foreach (AbilityModel abilityModel in _abilityModels)
                    abilityModel.Tick();
            }
        }

        private void TickAgility()
        {
            if (GetStat(StatType.CurrentAgility) < GetStat(StatType.MaxAgility))
                ChangeStatValue(StatType.CurrentAgility, GetStat(StatType.AgilityRestoreSpeed));

            if (GetStat(StatType.CurrentAgility) >= GetStat(StatType.MaxAgility))
            {
                _stats[StatType.CurrentAgility].Set(GetStat(StatType.MaxAgility));
                Prepared?.Invoke(this);
            }
        }
    }
}