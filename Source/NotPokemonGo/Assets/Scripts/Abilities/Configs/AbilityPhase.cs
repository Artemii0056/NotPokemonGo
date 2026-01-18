using System;
using System.Collections.Generic;
using Armaments;
using Cameras;
using Castaments;
using QTESystem;
using Units;
using UnityEngine;

namespace Abilities.Configs
{
    [Serializable]
    public class AbilityPhase
    {
        public AnimationClip AnimationClip;
        
        // public ArmamentSetup ArmamentSetup;
        // public CastamentSetup CastamentSetup;
        // public TargetMode TargetMode;
        // public CameraActionType CameraActionType;
        //
        public QteType QteType;

        /// <summary>
        /// If true, the phase finish will be gated by external async work (e.g. projectiles/QTE-in-flight)
        /// via a policy.
        /// Default: false (keeps backward compatibility for existing configs).
        /// </summary>
        public bool WaitForExternalCompletion;

        [field: SerializeField] public PhaseType PhaseType { get; set; }
        public List<ParticleSystemBySpawnType> ParticleSystemBySpawnType;
        
        public List<PhaseSignalAction> SignalActions = new();

        public int AnimationCashName => Animator.StringToHash(AnimationClip.name);
    }
}