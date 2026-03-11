using Abilities.Configs;
using DG.Tweening;
using Units;
using Units.Movement;
using UnityEngine;

namespace Services.AbilityServices.Executors
{
    public sealed class MoveActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly IUnitMover _unitMover;

        public MoveActionExecutor(IUnitMover unitMover) => _unitMover = unitMover;

        public bool CanExecute(PhaseSignalAction action) => action != null && action.HasMove;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate)
        {
            if (_unitMover == null || source == null)
                return;

            Vector3 dest = ResolveMoveDestination(action, source, target);
            float delay = Mathf.Max(0f, action.MoveDelay);

            Tween tween = null;

            void OnComplete()
            {
                // token release is handled by WithPhaseGate
            }

            if (action.MoveMode == MoveMode.Move)
                tween = _unitMover.MoveTo(source.transform, dest, action.MoveDuration, delay, OnComplete);
            else if (action.MoveMode == MoveMode.Jump)
                tween = _unitMover.JumpTo(source.transform, dest, action.MoveDuration, action.JumpPower, action.NumJumps, delay, OnComplete);

            // For instant moves (duration <= 0) tween will be null.
            tween?.WithPhaseGate(finishGate, $"MoveAction phase={phase.AnimationCashName}");
        }

        private static Vector3 ResolveMoveDestination(PhaseSignalAction phase, Unit source, Unit target)
        {
            switch (phase.MoveCommand)
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
                    return to - dir * Mathf.Max(0f, phase.StopDistance);
                }
                case MoveCommand.ToStartPosition:
                    return source.StartPosition;
                case MoveCommand.ToCustomPoint:
                    return phase.CustomPoint;
                default:
                    return source.transform.position;
            }
        }
    }
}
