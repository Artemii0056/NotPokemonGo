using System.Collections.Generic;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts
{
    [CreateAssetMenu(fileName = nameof(AbilitySO), menuName = "NewSystem/" + nameof(AbilitySO))]
    public class AbilitySO : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<AbilityStepSO> Steps = new List<AbilityStepSO>();
    }
}