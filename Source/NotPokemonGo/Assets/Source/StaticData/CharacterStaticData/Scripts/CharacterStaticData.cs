using System.Collections.Generic;
using Source.Characters.Configs;
using UnityEngine;

namespace Source.StaticData.CharacterStaticData.Scripts
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "StaticData/Character")]
    public class CharacterStaticData : ScriptableObject, ICharacterConfig
    {
        [field: SerializeField] public CharacterType Type { get; private set; }

        [Range(10, 100)] public float HealthPoints;
        [Range(75, 150)] public float ManaPoints;
        [Range(0, 100)] public float DodgeChance;
        [Range(0, 50)] public float Accuracy;
        [Range(10, 100)] public float ArmorChance;

        [SerializeField] private List<ISpell> _spells = new List<ISpell>();
        
        public List<ISpell> Spells => new List<ISpell>(_spells);
    }
}