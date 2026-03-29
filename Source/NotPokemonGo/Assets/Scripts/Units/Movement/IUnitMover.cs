using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Units.Movement
{
    public interface IUnitMover
    {
        UniTask MoveTo(Transform transform, Vector3 target, float duration);
        UniTask MoveTo(Transform transform, Vector3 target, float speed, CancellationToken token);
        Tween JumpTo(Transform transform, Vector3 target, float duration, float jumpPower = 1f, int numJumps = 1, float delay = 0f, Action onComplete = null);
    }
}
