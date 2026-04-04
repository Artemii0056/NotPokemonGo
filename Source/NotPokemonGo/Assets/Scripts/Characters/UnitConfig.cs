using System.Collections.Generic;
using Abilities.Configs;
using AbilityNew.Scripts;
using Characters.Configs;
using Stats;
using Units;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "StaticData/Unit")]
    public class UnitConfig : ScriptableObject
    {
        [field: SerializeField] public UnitType Type { get; private set; }
        [field: SerializeField] public Unit Prefab { get; private set; }
        [field: SerializeField] public AbilitySo CounterattackConfig { get; private set; }

        [SerializeField] private List<StatConfig> _stats = new List<StatConfig>();
        [SerializeField] private List<AbilityConfig> _abilityConfigs;
        [SerializeField] private List<AbilitySo> _abilitySO;

        public List<StatConfig> Stats => new List<StatConfig>(_stats);
        public List<AbilityConfig> AbilityConfigs => new List<AbilityConfig>(_abilityConfigs);
        public List<AbilitySo> AbilitySO => new List<AbilitySo>(_abilitySO);
    }
}