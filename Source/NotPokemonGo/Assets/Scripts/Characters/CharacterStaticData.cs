using System.Collections.Generic;
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

        public List<StatConfig> Stats => new List<StatConfig>(_stats);
    }
}