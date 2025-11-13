using System;
using System.Collections.Generic;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using Abilities.AbilitySteps;
using Cameras;
using QTESystem;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class AbilityPhase
    {
        [HideInInspector] public AnimationClip AnimationClip;
        
        [HideInInspector]public ArmamentSetup ArmamentSetup;
        [HideInInspector]public CastamentSetup CastamentSetup;
        [HideInInspector]public TargetMode TargetMode;
        [HideInInspector]public QteType QteType;
        [HideInInspector]public CameraActionType CameraActionType;
        [HideInInspector] [field: SerializeField] public PhaseType PhaseType { get; set; }
        
        public int AnimationCashName => Animator.StringToHash(AnimationClip.name);
        
        [SerializeReference]
        private List<AbilityStepData> _steps = new();

        public IReadOnlyList<AbilityStepData> Steps => _steps;
    }
}