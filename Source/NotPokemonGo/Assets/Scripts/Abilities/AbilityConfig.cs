using System.Collections.Generic;
using QTESystem;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(AbilityConfig), menuName = "StaticData/" + nameof(AbilityConfig))]
    public class AbilityConfig : ScriptableObject
    {
        [field: SerializeField] public AbilityType AbilityType { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public List<ParticleSystem> StartAnimationParticles { get; private set; }
        [field: SerializeField] public List<ParticleSystem> MiddleAnimationParticles { get; private set; }
        [field: SerializeField] public List<ParticleSystem> EndAnimationParticles { get; private set; }
        [field: SerializeField] public List<AbilityPhase> Phases { get; private set; }
        [field: SerializeField] public List<AbilityStatSetup> AbilityStatSetup { get; private set; }
    }
}