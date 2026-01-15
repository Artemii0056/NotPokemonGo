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
        public float MoveDuration = 0.35f;
        public float MoveDelay = 0.0f;
        public float StopDistance = 1.5f;
        public float JumpPower = 1.0f;
        public int NumJumps = 1;

        [Header("Camera (optional)")]
        public CameraCommand CameraCommand = CameraCommand.None;
        public float CameraBlendTimeout = 0.75f; // защита от “вечного бленда”

        public bool HasArmament => ArmamentSetup != null && ArmamentSetup.HasSetupData;
        public bool HasCastament => CastamentSetup != null && CastamentSetup.HasSetupData;

        public bool HasParticle => ParticlePrefab != null;
        public bool HasMove => MoveCommand != MoveCommand.None;
        public bool HasCamera => CameraCommand != CameraCommand.None;
    }
}