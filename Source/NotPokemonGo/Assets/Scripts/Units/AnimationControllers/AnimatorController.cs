using System;
using Abilities.Signals;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class AnimatorController : MonoBehaviour
    {
        private Animator _animator;

        public event Action Particle1;
        public event Action Particle2;
        public event Action Particle3;
        public event Action Attack1;
        public event Action Attack2;

        public event Action Finished;
        
        public event Action<PhaseSignal> Signal;

        private void Awake() =>
            _animator = GetComponent<Animator>();
        
        public void FlagSignal(int id)
        {
            var signal = PhaseSignalUtil.FromInt(id);
            
            if (signal == PhaseSignal.None)
                return;

            Signal?.Invoke(signal);
        }
        
        public void Play(int animationName) =>
            _animator.Play(animationName, 0, 0f);
        
        public void FlagParticleSystem1() =>
            Particle1?.Invoke();

        public void FlagParticleSystem2() =>
            Particle2?.Invoke();

        public void FlagParticleSystem3() =>
            Particle3?.Invoke();

        public void FlagAttack() => 
            Attack1?.Invoke();

        public void FlagAttack2() =>
            Attack2?.Invoke();

        public void FlagFinishAnimation() => 
            Finished?.Invoke();

        public float GetAnimationLength()
        {
            AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);

            AnimatorClipInfo[] currentClips = _animator.GetCurrentAnimatorClipInfo(0);
            AnimatorClipInfo[] nextClips = _animator.GetNextAnimatorClipInfo(0);

            if (currentClips.Length > 0)
                return currentClips[0].clip.length;

            if (nextClips.Length > 0)
                return nextClips[0].clip.length;

            return currentState.length;
        }

        public string GetAnimationName()
        {
            string animationName = string.Empty;
            
            AnimatorClipInfo[] currentClips = _animator.GetCurrentAnimatorClipInfo(0);

            if (currentClips.Length > 0)
            {
                animationName = currentClips[0].clip.name;
            }

            return animationName;
        }


        public void Pause()
        {
            _animator.speed = 0;
        }

        public void Continue()
        {
            _animator.speed = 1;
        }
    }
}