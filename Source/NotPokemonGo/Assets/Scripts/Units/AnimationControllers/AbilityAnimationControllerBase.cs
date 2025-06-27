using System;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class AbilityAnimationControllerBase : MonoBehaviour // TODO Сюда передать сервис 
    {
        //private IAbilityApplicatorService 
        public Animator _animator;
        private bool _isPlaying;

        public event Action ParticleSystem1Started;
        public event Action ParticleSystem2Started;
        public event Action ParticleSystem3Started;
        public event Action Attack1Started;

        public event Action Finished;

        public void Play(int animationName)
        {
            if (_isPlaying)
            {
                return;
            }

            _isPlaying = true;
            _animator.Play(animationName);
        }

        public void FlagParticleSystem1()
        {
            // AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // string animationName = stateInfo.shortNameHash.ToString();

            ParticleSystem1Started?.Invoke();
            //Двойной удар попробовать 
        }

        public void FlagParticleSystem2() =>
            ParticleSystem2Started?.Invoke();

        public void FlagParticleSystem3() =>
            ParticleSystem3Started?.Invoke();

        public void FlagAttack() =>
            Attack1Started?.Invoke();

        public void FlagFinishAnimation()
        {
            _isPlaying = false;
            Finished?.Invoke();
        }
    }
}