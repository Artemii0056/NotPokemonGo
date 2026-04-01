using System.Collections.Generic;
using Abilities;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts
{
    [CreateAssetMenu(fileName = nameof(AbilitySO), menuName = "NewSystem/" + nameof(AbilitySO))]
    public class AbilitySO : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<AbilityStepSO> Steps = new List<AbilityStepSO>();

        [field: SerializeField] public AbilityType Type { get; private set; }
        [field: SerializeField] public List<AbilityStatSetup> AbilityStatSetup { get; private set; }
    }
}