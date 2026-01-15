using System;
using System.Linq;
using Abilities.Configs;
using Abilities.Signals;
using Cameras;
using Castaments;
using DefaultNamespace;
using Units;
using Units.Movement;
using UnityEngine;

namespace Services.AbilityServices
{
    public sealed class AbilityPhaseService
    {
        private readonly ICastamentApplicator _castamentApplicator;
        private readonly ITargetSelector _targetSelector;
        private readonly IParticleSpawner _particleSpawner;
        private readonly IUnitMover _unitMover;
        private readonly ICameraService _camera;

        public event Action<ArmamentRequest> ArmamentRequested;

        public AbilityPhaseService(
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IParticleSpawner particleSpawner,
            IUnitMover unitMover,
            ICameraService camera)
        {
            _castamentApplicator = castamentApplicator;
            _targetSelector = targetSelector;
            _particleSpawner = particleSpawner;
            _unitMover = unitMover;
            _camera = camera;
        }

        public void OnSignal(AbilityPhase phase, Unit source, Unit target, PhaseSignal signal)
        {
            if (phase == null || source == null || target == null)
                return;

            var actions = phase.SignalActions;
            if (actions == null || actions.Count == 0)
                return;

            for (int i = 0; i < actions.Count; i++)
            {
                var a = actions[i];
                if (a == null) continue;
                if (a.Signal != signal) continue;

                if (a.HasParticle)
                {
                    var owner = a.ParticleOwner == ParticleOwner.Source ? source : target;
                    _particleSpawner.Spawn(owner, a.ParticleSpawnType, a.ParticlePrefab);
                }

                // Камера: если она должна “дожить” до конца — она сама закрывает фазу Finish
                if (a.HasCamera)
                    StartCamera(a, source, target);

                if (a.HasMove)
                    StartMove(a, source, target);

                if (a.HasArmament)
                {
                    var targets = _targetSelector.GetTargets(a.TargetMode, target).ToArray();
                    if (targets.Length > 0)
                        ArmamentRequested?.Invoke(new ArmamentRequest(phase, a, source, targets));
                }

                if (a.HasCastament)
                {
                    var targets = _targetSelector.GetTargets(a.TargetMode, target).ToArray();
                    if (targets.Length > 0)
                        _castamentApplicator.Apply(a.CastamentSetup, source, targets);
                }
            }
        }

        private void StartCamera(PhaseSignalAction a, Unit source, Unit target)
        {
            void OnComplete()
            {
                if (source != null && source.AnimatorController != null)
                    source.AnimatorController.FlagSignal((int)PhaseSignal.Finish);
            }

            _camera?.Play(a.CameraCommand, source, target, a.CameraBlendTimeout, OnComplete);
        }

        private void StartMove(PhaseSignalAction a, Unit source, Unit target)
        {
            Vector3 dest = ResolveMoveDestination(a, source, target);

            float duration = Mathf.Max(0.01f, a.MoveDuration);
            float delay = Mathf.Max(0f, a.MoveDelay);

            void OnComplete()
            {
                if (source != null && source.AnimatorController != null)
                    source.AnimatorController.FlagSignal((int)PhaseSignal.Finish);
            }

            if (a.MoveMode == MoveMode.Move)
                _unitMover.MoveTo(source.transform, dest, duration, delay, OnComplete);
            else
                _unitMover.JumpTo(source.transform, dest, duration, Mathf.Max(0f, a.JumpPower), Mathf.Max(1, a.NumJumps), delay, OnComplete);
        }

        private static Vector3 ResolveMoveDestination(PhaseSignalAction a, Unit source, Unit target)
        {
            switch (a.MoveCommand)
            {
                case MoveCommand.ToTargetStopPoint:
                {
                    Vector3 from = source.transform.position;
                    Vector3 to = target.transform.position;
                    Vector3 dir = to - from;
                    if (dir.sqrMagnitude < 0.0001f)
                        return from;
                    dir.Normalize();
                    return to - dir * Mathf.Max(0f, a.StopDistance);
                }

                case MoveCommand.ToStartPosition:
                    return source.StartPosition;

                case MoveCommand.ToCustomPoint:
                    return source.transform.position;

                default:
                    return source.transform.position;
            }
        }
    }
}
