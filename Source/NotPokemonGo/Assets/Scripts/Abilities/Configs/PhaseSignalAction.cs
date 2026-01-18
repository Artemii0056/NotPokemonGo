using System;
using Abilities.Signals;
using Armaments;
using Castaments;
using UnityEngine;

namespace Abilities.Configs
{
    public enum ParticleOwner { Source = 0, Target = 1 }
    public enum MoveMode { Move = 0, Jump = 1 }

    public enum MoveCommand
    {
        None = 0,
        ToTargetStopPoint = 1,
        ToStartPosition = 2,
        ToCustomPoint = 3,
    }

    public enum CameraCommand
    {
        None = 0,
        FocusOnSource = 1,
        FocusOnTarget = 2,
        Reset = 3,
    }

    [Serializable]
    public sealed class PhaseSignalAction
    {
        public PhaseSignal Signal;

        [Header("Targeting")]
        public TargetMode TargetMode = TargetMode.Single;

        [Header("Armament / Castament (optional)")]
        public ArmamentSetup ArmamentSetup;
        public CastamentSetup CastamentSetup;

        [Header("Particles (optional)")]
        public ParticleSystem ParticlePrefab;
        public ParticleSpawnType ParticleSpawnType = ParticleSpawnType.Default;
        public ParticleOwner ParticleOwner = ParticleOwner.Source;

        [Header("Movement (optional)")]
        public MoveCommand MoveCommand = MoveCommand.None;
        public MoveMode MoveMode = MoveMode.Jump;
        [Tooltip("World-space destination for MoveCommand.ToCustomPoint")]
        public Vector3 CustomPoint;
        public float MoveDuration = 0.35f;
        public float MoveDelay = 0.0f;
        public float StopDistance = 1.5f;
        public float JumpPower = 1.0f;
        public int NumJumps = 1;

        [Header("Camera (optional)")]
        public CameraCommand CameraCommand = CameraCommand.None;
        public float CameraBlendTimeout = 0.75f; // защита от “вечного бленда”
        
        [Header("Sound (optional)")]
        public AudioClip SfxClip;
        [Range(0f, 1f)] public float SfxVolume = 1f;
        [Tooltip("If true -> 2D sound, else -> 3D at owner position")]
        public bool Sfx2D = true;
        
        public enum TimeEffectType { None = 0, HitStop = 1, SlowMo = 2 }

        [Header("Time Effect (optional)")]
        public TimeEffectType TimeEffect = TimeEffectType.None;
        public float TimeScale = 0.1f;
        public float TimeDuration = 0.08f;

        public bool HasTimeEffect => TimeEffect != TimeEffectType.None;

        public bool HasSound => SfxClip != null;

        public bool HasArmament => ArmamentSetup != null && ArmamentSetup.HasSetupData;
        public bool HasCastament => CastamentSetup != null && CastamentSetup.HasSetupData;

        public bool HasParticle => ParticlePrefab != null;
        public bool HasMove => MoveCommand != MoveCommand.None;
        public bool HasCamera => CameraCommand != CameraCommand.None;
    }
}