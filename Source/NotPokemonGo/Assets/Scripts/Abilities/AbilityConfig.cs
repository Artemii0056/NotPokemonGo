using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(AbilityConfig), menuName = "StaticData/" + nameof(AbilityConfig))]
    public class AbilityConfig : ScriptableObject
    {
        [SerializeField] private AnimationClip _animationClip;
        [field: SerializeField] public AbilityType AbilityType { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        
        [field: SerializeField] public List<ParticleSystem> StartAnimationParticles { get; private set; }
        [field: SerializeField] public List<ParticleSystem> MiddleAnimationParticles { get; private set; }
        [field: SerializeField] public List<ParticleSystem> EndAnimationParticles { get; private set; }

        [field: SerializeField] public float Cost { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }

        [field: SerializeField] public TargetMode TargetMode { get; private set; } 
        
        [field: SerializeField] public List<AbilityPhase> Phases { get; private set; }
        
        public int AnimationHash => Animator.StringToHash(_animationClip.name);
    }
}