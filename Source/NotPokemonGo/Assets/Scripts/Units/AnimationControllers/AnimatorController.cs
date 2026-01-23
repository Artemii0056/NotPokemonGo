using System;
using UnityEngine;

namespace Units.AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorController : MonoBehaviour
    {
        private Animator _animator;

        public event Action<int> Signal;

        private void Awake() => 
            _animator = GetComponent<Animator>();

        public void Play(int stateHash) => 
            _animator.Play(stateHash, 0, 0f);

        public void FlagSignal(int id) => 
            Signal?.Invoke(id);

        public float GetAnimationLength()
        {
            var clips = _animator.GetCurrentAnimatorClipInfo(0);
            if (clips.Length > 0 && clips[0].clip != null)
                return clips[0].clip.length;

            return _animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }
}