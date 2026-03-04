using System.Collections.Generic;
using Abilities.MV;
using UnityEngine;

namespace Abilities.StepConfigs
{
    /// <summary>
    /// DATA-ONLY. Runtime behavior is built by factories.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(AbilityDefinitionConfig), menuName = "StaticData/Abilities/" + nameof(AbilityDefinitionConfig))]
    public sealed class AbilityDefinitionConfig : ScriptableObject
    {
        [field: SerializeField] public AbilityType AbilityType { get; private set; }
        [field: SerializeField] public List<AbilityStepConfig> Steps { get; private set; } = new();
    }
}
