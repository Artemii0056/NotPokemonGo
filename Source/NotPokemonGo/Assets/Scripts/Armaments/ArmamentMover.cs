using System;
using System.Collections;
using DG.Tweening;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Armaments
{
    public class ArmamentMover : IArmamentMover
    {
        public event Action<Armament, ArmamentMover> Reached;

        public void Move(Armament armament, bool isReturn = false)
        {
            if (isReturn)
                PlayDirectFlight(armament);
            else
                PlayArcFlight(armament);
        }

        private void PlayDirectFlight(Armament armament)
        {
            float duration = 1f;
            
            armament.transform.DOMove(armament.Target.transform.position, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Reached?.Invoke(armament, this));
        }

        private void PlayArcFlight(Armament armament)
        {
            float duration = 1f;
            float arcWidth = 3f;
            float arcHeight = 2f;

            bool leftArc = Random.Range(0, 2) == 1;

            Vector3 start = armament.transform.position;
            Vector3 end = armament.Target.transform.position;

            Vector3 mid = (start + end) / 2;

            Vector3 dir = (end - start).normalized;
            Vector3 side = Vector3.Cross(dir, Vector3.up).normalized;

            if (leftArc)
                side = -side;

            Vector3 control = mid + side * arcWidth + Vector3.up * arcHeight;

            Vector3[] path = { start, control, end };

            armament.transform.DOPath(path, duration, PathType.CatmullRom)
                .SetDelay(0.25f) 
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                {
                    Reached?.Invoke(armament, this);
                });
        }
    }
}