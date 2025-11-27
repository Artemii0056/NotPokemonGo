using System;
using System.Collections.Generic;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using Cameras;
using QTESystem;
using Units;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class AbilityPhase
    {
        public AnimationClip AnimationClip;
        public ArmamentSetup ArmamentSetup;
        public CastamentSetup CastamentSetup;
        public TargetMode TargetMode;
        public QteType QteType;
        public CameraActionType CameraActionType;

        [field: SerializeField] public PhaseType PhaseType { get; set; }
        public List<ParticleSystemBySpawnType> ParticleSystemBySpawnType;

        public int AnimationCashName => Animator.StringToHash(AnimationClip.name);
    }
}