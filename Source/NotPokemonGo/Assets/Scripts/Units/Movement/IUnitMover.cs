using System;
using UnityEngine;

namespace Units.Movement
{
    public interface IUnitMover
    {
        bool IsMoving { get; }
        void MoveTo(Transform transform, Vector3 target, float duration, float delay = 0f, Action onComplete = null);
        void JumpTo(Transform transform, Vector3 target, float duration, float jumpPower = 1f, int numJumps = 1, float delay = 0f, Action onComplete = null);
        void Stop();
    }
}