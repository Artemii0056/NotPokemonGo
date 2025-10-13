using System;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class UnitAnimatorController : MonoBehaviour
    {
        private Animator _animator;

        public event Action ParticleSystem1Started;
        public event Action ParticleSystem2Started;
        public event Action ParticleSystem3Started;
        public event Action Attack1Started;
        public event Action Attack2Started;

        public event Action Finished;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void Play(int animationName) => 
            _animator.Play(animationName);

        public void FlagParticleSystem1() => 
            ParticleSystem1Started?.Invoke();

        public void FlagParticleSystem2() =>
            ParticleSystem2Started?.Invoke();

        public void FlagParticleSystem3() =>
            ParticleSystem3Started?.Invoke();

        public void FlagAttack() =>
            Attack1Started?.Invoke();
        
        public void FlagAttack2() =>
            Attack2Started?.Invoke();

        public void FlagFinishAnimation() => 
            Finished?.Invoke();
    }
}