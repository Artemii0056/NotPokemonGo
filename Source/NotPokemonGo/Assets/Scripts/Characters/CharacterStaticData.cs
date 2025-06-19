using System.Collections.Generic;
using Abilities;
using Characters.Configs;
using Stats;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "StaticData/Character")]
    public class CharacterStaticData : ScriptableObject
    {
        [field: SerializeField] public CharacterType Type { get; private set; }

        [SerializeField] private List<StatConfig> _stats = new List<StatConfig>();

        [SerializeField] private List<AbilityConfig> _abilityConfigs;

        public List<StatConfig> Stats => new List<StatConfig>(_stats);
        public List<AbilityConfig> AbilityConfigs => new List<AbilityConfig>(_abilityConfigs);
    }
}