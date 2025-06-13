using System.Collections.Generic;
using Characters.Configs;
using Characters.Configs.Stats;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "StaticData/Character")]
    public class CharacterStaticData : ScriptableObject, ICharacterConfig
    {
        [field: SerializeField] public CharacterType Type { get; private set; }
        
        //[SerializeField] private string name;

        public StatConfig Health;
        public StatConfig Mana;
        public StatConfig DodgeChance;
        public StatConfig Accuracy;
        public StatConfig ArmorChance;

        private List<StatConfig> _stats;

        public List<StatConfig> Stats => new List<StatConfig>(_stats);

        [SerializeField] private List<ISpell> _spells = new List<ISpell>();

        public List<ISpell> Spells => new List<ISpell>(_spells);

        private void OnEnable()
        {
            _stats = new List<StatConfig>();
            _stats.Add(Health);
            _stats.Add(Mana);
            _stats.Add(DodgeChance);
            _stats.Add(Accuracy);
            _stats.Add(ArmorChance);
        }
    }
}