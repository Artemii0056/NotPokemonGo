using Units;
using UnityEngine;

namespace AbilityNew.Scripts.Presentation
{
    public sealed class AbilityPresentationContext
    {
        public Unit Caster;
        public Unit Target;
        public AbilitySO Ability;
        public AbilityPresentationSignal Signal;

        public ParticleSpawnType SpawnType = ParticleSpawnType.Default;
        public Transform ExplicitTransform;
    }
}