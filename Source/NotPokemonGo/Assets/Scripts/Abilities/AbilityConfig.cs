using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(AbilityConfig), menuName = "StaticData/" + nameof(AbilityConfig))]
    public class AbilityConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: SerializeField] public TargetMode TargetMode { get; private set; }
        [field: SerializeField] public float Cost { get; private set; }

        [field: SerializeField] public ArmamentSetup ArmamentSetup{ get; private set; }
        [field: SerializeField] public CastamentSetup CastamentSetup { get; private set; }
        [field: SerializeField] public AbilityView Prefab { get; private set; }
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
    }
}