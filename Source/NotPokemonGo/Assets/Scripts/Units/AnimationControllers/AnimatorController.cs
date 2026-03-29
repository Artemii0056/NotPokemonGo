using System;
using System.IO;
using UnityEngine;

namespace Units.AnimationControllers
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorController : MonoBehaviour
    {
        private Animator _animator;

        public event Action<int> Signal;

        private void Awake()
        {
            Trace("AnimatorController.Awake START");
            _animator = GetComponent<Animator>();
            Trace($"AnimatorController.Awake animator={_animator}");
        }

        public void Play(int stateHash)
        {
            Trace($"AnimatorController.Play ENTER hash={stateHash} animator={_animator}");
            _animator.Play(stateHash, 0, 0f);
            Trace("AnimatorController.Play EXIT");
        }

        public void FlagSignal(int id) => 
            Signal?.Invoke(id);

        public float GetAnimationLength()
        {
            var clips = _animator.GetCurrentAnimatorClipInfo(0);
            
            if (clips.Length > 0 && clips[0].clip != null)
                return clips[0].clip.length;

            return _animator.GetCurrentAnimatorStateInfo(0).length;
        }
        
        private void Trace(string message)
        {
            var path = Path.Combine(Application.persistentDataPath, "ability_trace.log");
            File.AppendAllText(path, $"{DateTime.Now:HH:mm:ss.fff} | {message}\n");
        }
    }
}