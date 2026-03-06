using System.Collections.Generic;
using AbsolutelyNewPerfectAbilitySystem.Configs;
using UnityEngine;

namespace AbsolutelyNewPerfectAbilitySystem
{
    [CreateAssetMenu(fileName = nameof(AbilitySO), menuName = "NewSystem/" + nameof(AbilitySO))]
    public class AbilitySO : ScriptableObject
    {
        [SerializeReference]
        public List<AbilityStepSO> Steps = new List<AbilityStepSO>();
    }
}