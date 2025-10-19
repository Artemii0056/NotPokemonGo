using System;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using QTESystem;
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
        [field: SerializeField] public PhaseType PhaseType { get; set; }
        
        public int AnimationCashName => Animator.StringToHash(AnimationClip.name);
    }
}