// =======================
// MoveActionExecutor.cs
// =======================
using System;
using Abilities.Configs;
using Abilities.Runtime;
using Abilities.Signals;
using Units;
using Units.Movement;
using UnityEngine;

namespace Services.AbilityServices.Executors
{
    public sealed class MoveActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly IUnitMover _unitMover;

        public MoveActionExecutor(IUnitMover unitMover) =>
            _unitMover = unitMover;

        public bool CanExecute(PhaseSignalAction action) =>
            action != null && action.HasMove;

        public bool Execute(
            AbilityPhase phase,
            PhaseSignalAction action,
            Unit source,
            Unit target,
            PhaseGate finishGate,
            Action tryCompleteFinish)
        {
            if (_unitMover == null || source == null)
                return false;

            Vector3 dest = ResolveMoveDestination(action, source, target);

            // ⚠️ В идеале duration берёшь из action/клипа. Пока оставим как у тебя.
            float duration = source.AnimatorController.GetAnimationLength();
            float delay = 0f;

            var token = finishGate.Acquire($"MoveActionExecutor phase={phase.AnimationCashName}");
            bool done = false;

            void OnComplete()
            {
                if (done) return;
                done = true;

                // ✅ Никаких FlagSignal(Finish) из кода.
                // ✅ Движение просто говорит "я закончил" через Gate.
                token.Dispose();
                tryCompleteFinish?.Invoke();
            }

            if (action.MoveMode == MoveMode.Move)
            {
                _unitMover.MoveTo(source.transform, dest, 0.75f, delay, OnComplete);
            }
            else
            {
                _unitMover.MoveTo(source.transform, dest, duration, delay, OnComplete);
                // _unitMover.JumpTo(... OnComplete);
            }

            return true;
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
