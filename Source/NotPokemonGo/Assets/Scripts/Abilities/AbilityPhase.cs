using System;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class AbilityPhase
    {
        public AnimationClip AnimationClip;
        
        public ArmamentSetup ArmamentSetup;
        public CastamentSetup CastamentSetup;
        
        public bool IsMelee;
        public bool IsMovementPhase;
        public bool IsReturnPhase;
        public TargetMode TargetMode;
    }
}