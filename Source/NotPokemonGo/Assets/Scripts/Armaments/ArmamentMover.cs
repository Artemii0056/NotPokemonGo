using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Armaments
{
    public class ArmamentMover : IArmamentMover
    {
        public Armament Armament { get; private set; }
        public float Duration { get; private set; }
        
        public event Action<IArmamentMover> Launched;
        public event Action<IArmamentMover> Reached;
        
        public void Move(Armament armament)
        {
            Armament = armament;
            
            switch (armament.FlyingType)
            {
                case ArmamentFlyingType.Arc:
                    PlayArcFlight(armament);
                    break;

                case ArmamentFlyingType.Direct:
                    PlayDirectFlight(armament);
                    break;

                case ArmamentFlyingType.Laser:
                    PlayLaserFlight(armament);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayDirectFlight(Armament armament)
        {
            float duration = 1f; //TODO В конфиг
            Duration = duration;

            armament.transform.DOMove(armament.Target.transform.position, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Reached?.Invoke(this));
        }

        private void PlayArcFlight(Armament armament)
        {
            float duration = 1f;
            Duration = duration;
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
                .SetDelay(0.25f)//TODO В конфиг
                .OnStart(() => Launched?.Invoke(this))
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                {
                    Reached?.Invoke(this);
                });
        }

        private void PlayLaserFlight(Armament armament)
        {
            float duration = .05f;
            Duration = duration;
            
            DOTween.Sequence()
                .Append(
                    armament.transform
                        .DOMove(armament.Target.transform.position, duration)
                        .SetEase(Ease.Linear)
                )
                .AppendInterval(0.25f)//TODO В конфиг
                .OnComplete(() => Reached?.Invoke(this));
        }
    }
}