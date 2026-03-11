using System.Collections.Generic;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem.Scripts
{
    [CreateAssetMenu(fileName = nameof(AbilitySO), menuName = "NewSystem/" + nameof(AbilitySO))]
    public class AbilitySO : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<AbilityStepSO> Steps = new List<AbilityStepSO>();
    }
}