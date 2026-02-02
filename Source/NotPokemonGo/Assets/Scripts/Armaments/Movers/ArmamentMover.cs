using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Armaments.Movers
{
    public class ArmamentMover : IArmamentMover
    {
        public event Action<IArmamentMover> Launched;
        public event Action<IArmamentMover> Reached;

        public ArmamentMover(Armament armament) =>
            Armament = armament;
        
        public Armament Armament { get; private set; }
        public float Duration { get; private set; }

        public void Move()
        {
            switch (Armament.FlyingType)
            {
                case ArmamentFlyingType.Arc:
                    PlayArcFlight();
                    break;

                case ArmamentFlyingType.Direct:
                    PlayDirectFlight();
                    break;

                case ArmamentFlyingType.Laser:
                    PlayLaserFlight();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayDirectFlight()
        {
            Duration = 1f;

            Launched?.Invoke(this);

            Armament.transform.DOMove(Armament.Target.transform.position, Duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Reached?.Invoke(this));
        }

        private void PlayArcFlight()
        {
            Duration = 1f;
            float arcWidth = 3f;
            float arcHeight = 2f;

            bool leftArc = Random.Range(0, 2) == 1;

            Vector3 start = Armament.transform.position;
            Vector3 end = Armament.Target.transform.position;

            Vector3 mid = (start + end) / 2;

            Vector3 dir = (end - start).normalized;
            Vector3 side = Vector3.Cross(dir, Vector3.up).normalized;

            if (leftArc)
                side = -side;

            Vector3 control = mid + side * arcWidth + Vector3.up * arcHeight;

            Vector3[] path = { start, control, end };

            Armament.transform.DOPath(path, Duration, PathType.CatmullRom)
                .OnStart(() => Launched?.Invoke(this))
                .SetEase(Ease.InExpo)
                .OnComplete(() => Reached?.Invoke(this));
        }

        private void PlayLaserFlight()
        {
            Duration =  .05f;

            Launched?.Invoke(this);

            DOTween.Sequence()
                .Append(Armament.transform.DOMove(Armament.Target.transform.position, Duration).SetEase(Ease.Linear))
                .AppendInterval(Armament.Setup.Duration) //TODO Вот это влияет на продолжительность линии
                .OnComplete(() => Reached?.Invoke(this));
        }
    }
}