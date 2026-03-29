using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Units.Movement
{
    public sealed class UnitMover : IUnitMover
    {
        private Tween _tween;
        public bool IsMoving { get; private set; }

        public UniTask MoveTo(Transform transform, Vector3 target, float duration)
        {
            if (transform == null)
                throw new NullReferenceException("transform is null");

            Stop();

            if (duration <= 0f)
            {
                transform.position = target;
                IsMoving = false;
                return UniTask.CompletedTask;
            }

            IsMoving = true;
            
            var tcs = new UniTaskCompletionSource();

            _tween = transform.DOMove(target, duration)
                .SetEase(Ease.Linear)
                .OnKill(() =>
                {
                    Clear();
                    tcs.TrySetResult();
                })
            .OnComplete(() =>
                {
                    Clear();
                    tcs.TrySetResult();
                });

            return tcs.Task;
        }

        public async UniTask  MoveTo(Transform transform, Vector3 target, float speed, CancellationToken token)
        {
            float duration = Vector3.Distance(transform.position, target) / speed;

            Tween tween = transform.DOMove(target, duration).SetEase(Ease.Linear);

            await tween.AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(token);
        }

        public Tween JumpTo(Transform transform, Vector3 target, float duration, float jumpPower = 0f, int numJumps = 1, float delay = 0f, Action onComplete = null)
        {
            if (transform == null)
                throw new NullReferenceException("transform is null");

            Stop();

            if (duration <= 0f)
            {
                transform.position = target;
                IsMoving = false;
                onComplete?.Invoke();
                return null;
            }

            IsMoving = true;
            transform.DOKill();

            _tween = transform.DOJump(target, jumpPower, Mathf.Max(1, numJumps), duration)
                .SetDelay(Mathf.Max(0f, delay))
                .SetEase(Ease.Linear)
                .OnKill(Clear)
                .OnComplete(() =>
                {
                    Clear();
                    onComplete?.Invoke();
                });

            return _tween;
        }

        private void Stop()
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
