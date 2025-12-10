using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Configs
{
    [CreateAssetMenu(fileName = nameof(AbilityConfig), menuName = "StaticData/" + nameof(AbilityConfig))]
    public class AbilityConfig : ScriptableObject
    {
        [field: SerializeField] public AbilityType AbilityType { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public List<AbilityPart> Parts { get; private set; }
        [field: SerializeField] public List<AbilityStatSetup> AbilityStatSetup { get; private set; }
        [field: SerializeField] public Interruptibility Interruptibility { get; private set; }
    }
}