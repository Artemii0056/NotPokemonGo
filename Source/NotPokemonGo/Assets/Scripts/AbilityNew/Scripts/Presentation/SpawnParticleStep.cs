using System;
using UnityEngine;

namespace AbilityNew.Scripts.Presentation
{
    [Serializable]
    public sealed class SpawnParticleStep : PresentationStep
    {
        public PresentationAnchor Anchor;
        public ParticleSystem Prefab;
        public ParticleSpawnType SpawnType = ParticleSpawnType.Default;
    }
}