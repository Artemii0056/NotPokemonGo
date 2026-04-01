using System;
using AbilityNew.Scripts.Configs;
using AbilityNew.Scripts.Presentation;

namespace AbilityNew.Scripts.Steps.Presentation
{
    [Serializable]
    public sealed class EmitPresentationSignalStep : AbilityStepSO
    {
        public AbilityPresentationSignal Signal;
        public ParticleSpawnType SpawnType = ParticleSpawnType.Default;
    }
}