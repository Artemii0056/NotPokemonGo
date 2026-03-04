using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Armaments.Movers
{
    public class ArmamentMover : IArmamentMover, IAbilityScopeOwnedMover
    {
        public event Action<IArmamentMover> Launched;
        public event Action<IArmamentMover> Reached;

        private int _scopeId;
        private Tween _flightTween;
        private bool _reachedRaised;

        public ArmamentMover(Armament armament) => Armament = armament;

        public Armament Armament { get; private set; }
        public float Duration { get; private set; }

        public void SetScopeId(int scopeId) => _scopeId = scopeId;

        public void Move()
        {
            _reachedRaised = false;

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

        private void BindOwnership(Tween tween)
        {
            _flightTween = tween;

            if (tween == null)
                return;

            if (_scopeId != 0)
                tween.SetId(_scopeId);

            tween.OnKill(OnTweenKilledOrCompleted);
            tween.OnComplete(OnTweenKilledOrCompleted);
        }

        private void OnTweenKilledOrCompleted()
        {
            // IMPORTANT: if ability is cancelled and tween is killed, we still want to unblock steps
            // that wait for Reached (otherwise deadlock / stuck phase).
            TryRaiseReached();
        }

        private void TryRaiseReached()
        {
            if (_reachedRaised)
                return;

            _reachedRaised = true;
            Reached?.Invoke(this);
        }

        private void PlayDirectFlight()
        {
            Duration = .5f;
            Launched?.Invoke(this);

            var tween = Armament.transform
                .DOMove(Armament.Target.transform.position, Duration)
                .SetEase(Ease.Linear);

            BindOwnership(tween);
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

            var tween = Armament.transform
                .DOPath(path, Duration, PathType.CatmullRom)
                .SetEase(Ease.InExpo)
                .OnStart(() => Launched?.Invoke(this));

            BindOwnership(tween);
        }

        private void PlayLaserFlight()
        {
            Duration = .05f;
            Launched?.Invoke(this);

            var seq = DOTween.Sequence()
                .Append(Armament.transform.DOMove(Armament.Target.transform.position, Duration).SetEase(Ease.Linear))
                .AppendInterval(Armament.Setup.FlyDuration);

            BindOwnership(seq);
        }
    }
}
