using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.Bennet;
using Abilities.MV;
using Characters.Configs;
using Cinemachine;
using Platoons;
using Stats;
using Statuses;
using Units.AnimationControllers;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private List<AbilityAnchor> abilityAnchors;

        [field: SerializeField] public AnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public HighlightContainer HighlightContainer { get; private set; }

        public Transform abilityPos;
        public List<Transform> AbilitiesPositions;
        public CinemachineVirtualCamera virtualCamera;

        private List<Status> _imposedStatuses = new List<Status>(); // отдельный сервис
        private List<AbilityModel> _abilityModels = new List<AbilityModel>();
        private Dictionary<StatType, StatSetup> _stats = new Dictionary<StatType, StatSetup>();

        public Vector3 StartPosition { get; private set; }
        public PlatoonType PlatoonType { get; private set; }
        public List<Status> ImposedStatuses => _imposedStatuses.ToList();
        public List<AbilityModel> AbilityModels => _abilityModels.ToList();
        public List<AbilityAnchor> AbilityAnchors => abilityAnchors.ToList();
        public Dictionary<StatType, StatSetup> Stats => new(_stats);

        public int Id { get; private set; }

        public bool IsAlive => _stats[StatType.Health].CurrentValue > 0;

        public event Action Ticked;

        public event Action<Status> StatusAdded;
        public event Action<Status> StatusRemoved;

        public event Action<Unit> Prepared;
        public event Action<float, float> AgilityChanged;
        public event Action<float, float> HealthChanged;

        public event Action<Unit> Death;

        public IAbilityHandler AbilityHandler { get; private set; }

        public void Construct(
            List<StatConfig> statConfig,
            PlatoonType platoonType,
            int id)
        {
            Id = id;
            PlatoonType = platoonType;

            foreach (var statSetup in statConfig)
                _stats.Add(statSetup.StatsType, new StatSetup(statSetup));

            foreach (StatSetup stat in _stats.Values)
                stat.CurrentValueChanged += OnStatValueChanged;

            HealthChanged?.Invoke(GetStat(StatType.Health), GetStat(StatType.MaxHealth));

            StartPosition = transform.position;
        }

        public void Construct(
            Dictionary<StatType, StatSetup> stats,
            PlatoonType platoonType,
            int id)
        {
            Id = id;
            _stats = stats;

            PlatoonType = platoonType;

            foreach (StatSetup stat in _stats.Values)
                stat.CurrentValueChanged += OnStatValueChanged;

            HealthChanged?.Invoke(GetStat(StatType.Health), GetStat(StatType.MaxHealth));
        }

        private void OnDestroy()
        {
            foreach (StatSetup stat in _stats.Values)
                stat.CurrentValueChanged -= OnStatValueChanged;
        }

        public float GetStat(StatType statType) =>
            _stats[statType].CurrentValue;

        public bool HasStatus(StatusType statusType)
        {
            foreach (Status status in _imposedStatuses)
            {
                if (statusType == status.Setup.Type)
                    return true;
            }

            return false;
        }

        public void ChangeStatValue(float value, StatType statType)
        {
            //Debug.Log(statType + " " + value);
            _stats[statType].SetValue(value);
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
            _stats[StatType.CurrentAgility].SetValue(0);

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
                // foreach (AbilityModel abilityModel in _abilityModels)
                //     abilityModel.Tick();
            }
        }

        private void TickAgility()
        {
            if (GetStat(StatType.CurrentAgility) < GetStat(StatType.MaxAgility))
            {
                float newValue = GetStat(StatType.CurrentAgility) + GetStat(StatType.AgilityRestoreSpeed);
                ChangeStatValue(newValue, StatType.CurrentAgility);
            }

            if (GetStat(StatType.CurrentAgility) >= GetStat(StatType.MaxAgility))
            {
                _stats[StatType.CurrentAgility].SetValue(GetStat(StatType.MaxAgility));
                Prepared?.Invoke(this);
            }
        }

        private void OnStatValueChanged(float value, StatType statType)
        {
            switch (statType)
            {
                case StatType.Health:
                    HealthChanged?.Invoke(value, GetStat(StatType.MaxHealth));
                    break;

                case StatType.CurrentAgility:
                    AgilityChanged?.Invoke(value, GetStat(StatType.MaxAgility));
                    break;
            }
        }

        public void RememberAbility(IAbilityHandler activeAbilityHandlers) =>
            AbilityHandler = activeAbilityHandlers;

        public void SetStartPosition(Vector3 pos)
        {
            StartPosition = pos;
        }
    }
}