using System.Collections.Generic;
using UnityEngine;

namespace Source.Characters.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Characters/CharacterConfig")]
    public class CharacterConfig : ScriptableObject, ICharacterConfig
    {
        [field: SerializeField] public CharacterType Type { get; private set; }
        
        [field: SerializeField] public float HealthPoints { get; private set; }
        [field: SerializeField] public float ManaPoints { get; private set; }
        [field: SerializeField] public float DodgeChance { get; private set; }
        [field: SerializeField] public float Accuracy { get; private set; }
        [field: SerializeField] public float ArmorChance { get; private set; }

        [SerializeField] private List<ISpell> _spells = new List<ISpell>();
        
        public List<ISpell> Spells => new List<ISpell>(_spells);
    }
}