using System;
using AbilityNew.Scripts.Configs;
using UnityEngine;

namespace AbilityNew.Scripts.Steps.Presentation
{
    [Serializable]
    public class SpawnVfxStep  : AbilityStepSO
    {
        public ParticleSpawnType ParticleSpawnType;
        public ParticleSystem ParticleSystem;
    }
}