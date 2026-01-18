using System;
using Abilities.Configs;
using Abilities.Runtime;
using Units;
using Units.Movement;
using UnityEngine;

namespace Services.AbilityServices.Executors
{
    public sealed class MoveActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly IUnitMover _unitMover;

        public MoveActionExecutor(IUnitMover unitMover)
        {
            _unitMover = unitMover;
        }

        public bool CanExecute(PhaseSignalAction action) => action != null && action.HasMove;

        public bool Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseFinishGate finishGate, Action tryCompleteFinish)
        {
            if (_unitMover == null || source == null)
                return false;

            Vector3 dest = ResolveMoveDestination(action, source, target);

            float duration = Mathf.Max(0.01f, action.MoveDuration);
            float delay = Mathf.Max(0f, action.MoveDelay);

            var token = finishGate.Acquire();

            void OnComplete()
            {
                token.Dispose();
                tryCompleteFinish?.Invoke();
            }

            if (action.MoveMode == MoveMode.Move)
            {
                _unitMover.MoveTo(source.transform, dest, duration, delay, OnComplete);
            }
            else
            {
                _unitMover.JumpTo(
                    source.transform,
                    dest,
                    duration,
                    Mathf.Max(0f, action.JumpPower),
                    Mathf.Max(1, action.NumJumps),
                    delay,
                    OnComplete);
            }

            return true;
        }

        private static Vector3 ResolveMoveDestination(PhaseSignalAction a, Unit source, Unit target)
        {
            switch (a.MoveCommand)
            {
                case MoveCommand.ToTargetStopPoint:
                {
                    if (target == null)
                        return source.transform.position;

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
                    return a.CustomPoint;

                default:
                    return source.transform.position;
            }
        }
    }
}
