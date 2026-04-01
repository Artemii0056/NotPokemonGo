using System;
using Services.Cameras;
using UnityEngine;

namespace AbilityNew.Presentation
{
    public enum PresentationActionType
    {
        SpawnParticleOnCaster,
        SpawnParticleOnTarget,
        SpawnParticleOnExplicitTransform,

        PlaySound2D,
        PlaySoundOnCaster,
        PlaySoundOnTarget,
        PlaySoundOnExplicitTransform,

        CameraShake
    }

    [Serializable]
    public sealed class PresentationAction
    {
        public PresentationActionType Type;

        public ParticleSystem ParticlePrefab;
        public AudioClip AudioClip;

        public ParticleSpawnType SpawnType = ParticleSpawnType.Default;

        public float Volume = 1f;

        public float ShakeAmplitude = 1f;
        public float ShakeFrequency = 1f;
        public float ShakeDuration = 0.2f;
    }
}