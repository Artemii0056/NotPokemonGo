using DG.Tweening;
using UnityEngine;

namespace Units.Movement
{
    public sealed class UnitMover : IUnitMover
    {
        private Tween _tween;
        public bool IsMoving { get; private set; }

        public void MoveTo(Transform transform, Vector3 target, float duration, float delay = 0f)
        {
            if (transform == null) 
            { 
                Stop(); 
                return;
            }

            Stop();

            if (duration <= 0f)
            {
                transform.position = target;
                IsMoving = false;
                return;
            }

            IsMoving = true;

            _tween = transform.DOMove(target, duration)
                .SetDelay(Mathf.Max(0f, delay))
                .SetEase(Ease.Linear)
                .OnKill(Clear)
                .OnComplete(Clear);
        }

        public void JumpTo(Transform transform, Vector3 target, float duration, float jumpPower = 1f, int numJumps = 1, float delay = 0f)
        {
            if (transform == null) 
            { 
                Stop();
                return;
            }

            Stop();

            if (duration <= 0f)
            {
                transform.position = target;
                IsMoving = false;
                return;
            }

            IsMoving = true;

            transform.DOKill();

            _tween = transform.DOJump(target, jumpPower, Mathf.Max(1, numJumps), duration)
                .SetDelay(Mathf.Max(0f, delay))
                .SetEase(Ease.InQuad)
                .OnKill(Clear)
                .OnComplete(Clear);
        }

        public void Stop()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }

            IsMoving = false;
        }

        private void Clear()
        {
            IsMoving = false;
            _tween = null;
        }
    }
}