using System.Collections.Generic;
using Characters.Configs;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "StaticData/Character")]
    public class CharacterStaticData : ScriptableObject, ICharacterConfig
    {
        [field: SerializeField] public CharacterType Type { get; private set; }

        public Stat Health;
        public Stat Mana;
        public Stat DodgeChance;
        public Stat Accuracy;
        public Stat ArmorChance;

        private List<Stat> _stats;

        public List<Stat> Stats => new List<Stat>(_stats);

        [SerializeField] private List<ISpell> _spells = new List<ISpell>();

        public List<ISpell> Spells => new List<ISpell>(_spells);

        private void OnEnable()
        {
            _stats = new List<Stat>();
            _stats.Add(Health);
            _stats.Add(Mana);
            _stats.Add(DodgeChance);
            _stats.Add(Accuracy);
            _stats.Add(ArmorChance);
        }
    }
}