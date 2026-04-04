using System.Collections.Generic;
using Abilities;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts
{
    [CreateAssetMenu(fileName = nameof(AbilitySo), menuName = "NewSystem/" + nameof(AbilitySo))]
    public class AbilitySo : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; } //TODO Выпилить бы 
        [field: SerializeField] public AbilityType Type { get; private set; }
        
        [SerializeReference, SubclassSelector]
        public List<AbilityStepSO> Steps = new List<AbilityStepSO>();

        [field: SerializeField] public List<AbilityStatSetup> AbilityStatSetup { get; private set; }
    }
}