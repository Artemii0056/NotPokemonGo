using System.Collections.Generic;
using Source.Characters.Configs;
using UnityEngine;

namespace Source.StaticData.CharacterStaticData.Scripts
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
        
       // public List<Stat> Stats = new List<Stat>(Health, Mana, DodgeChance, Accuracy, ArmorChance);
       
       public List<Stat> Stats;

        [SerializeField] private List<ISpell> _spells = new List<ISpell>();

        public List<ISpell> Spells => new List<ISpell>(_spells);
    }
}