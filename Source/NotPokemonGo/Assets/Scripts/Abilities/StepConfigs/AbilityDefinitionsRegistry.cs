using System.Collections.Generic;
using Abilities.MV;
using UnityEngine;

namespace Abilities.StepConfigs
{
    /// <summary>
    /// DATA-ONLY registry for step-based abilities.
    /// Place an instance in Resources as "AbilityDefinitionsRegistry" for quick bootstrap.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(AbilityDefinitionsRegistry), menuName = "StaticData/Abilities/" + nameof(AbilityDefinitionsRegistry))]
    public sealed class AbilityDefinitionsRegistry : ScriptableObject
    {
        [SerializeField] private List<AbilityDefinitionConfig> _definitions = new();

        public bool TryGet(AbilityType type, out AbilityDefinitionConfig definition)
        {
            for (int i = 0; i < _definitions.Count; i++)
            {
                var d = _definitions[i];
                
                if (d != null && d.AbilityType == type)
                {
                    definition = d;
                    return true;
                }
            }

            definition = null;
            return false;
        }
    }
}
