using System;
using DG.Tweening;
using UnityEngine;

namespace Units.Movement
{
    public interface IUnitMover
    {
        Tween MoveTo(Transform transform, Vector3 target, float duration, float delay = 0f, Action onComplete = null);
        Tween JumpTo(Transform transform, Vector3 target, float duration, float jumpPower = 1f, int numJumps = 1, float delay = 0f, Action onComplete = null);
    }
}
