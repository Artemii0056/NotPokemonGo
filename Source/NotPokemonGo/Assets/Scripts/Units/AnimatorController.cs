using UnityEngine;

namespace Units
{
    public class AnimatorController
    {
        private readonly Animator _animator;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void PlayAnimation(string animationName)
        {
            _animator.Play(animationName);
        }
    }
}