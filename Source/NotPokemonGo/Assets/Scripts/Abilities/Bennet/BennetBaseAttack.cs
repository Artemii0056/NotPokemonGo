using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.MV;
using Services;
using Units;
using Units.AnimationControllers;
using Units.Movement;
using UnityEngine;

namespace Abilities.Bennet
{
    public sealed class BennetBaseAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IUnitMover _unitMover;
        private readonly List<AbilityPart> _parts;

        private AnimatorController _animatorController;
        private UnitAnimatorTrigger _animatorTrigger;

        private Unit _source;
        private Unit _target;

        private Coroutine _routine;
        private bool _animationPlaying;
        private bool _stopped;

        private Vector3 _startPosition;

        private const float StopDistance = 1.5f;

        public BennetBaseAttack(
            AbilityModel abilityModel,
            ICoroutineRunner coroutineRunner,
            IUnitMover unitMover)
        {
            _coroutineRunner = coroutineRunner;
            _unitMover = unitMover;

            _parts = abilityModel.Parts;
            Interruptibility = abilityModel.Interruptibility;
        }

        public event Action<IAbilityHandler> Finished;
        public Interruptibility Interruptibility { get; }

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _animatorController = source.AnimatorController;
            _animatorTrigger = source.AnimatorTrigger;

            _startPosition = source.transform.position;

            _stopped = false;
            _routine = _coroutineRunner.StartCoroutine(Execute());
        }

        public void Stop()
        {
            _stopped = true;

            if (_routine != null)
                _coroutineRunner.StopCoroutine(_routine);

            _unitMover.Stop();
        }

        private IEnumerator Execute()
        {
            foreach (var part in _parts)
            {
                foreach (var phase in part.AbilityPhases)
                {
                    yield return ExecutePhase(phase);

                    if (_stopped)
                        yield break;
                }
            }

            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _animatorTrigger.SetTarget(_target);
            _animatorTrigger.SetPhase(phase);

            _animatorController.Play(phase.AnimationCashName);

            switch (phase.PhaseType)
            {
                case PhaseType.IsMovementPhase:
                    yield return JumpTo(CalculateTargetPosition(_source.transform.position, _target.transform.position));
                    yield break;

                case PhaseType.IsReturnPhase:
                    yield return JumpTo(_startPosition);
                    yield break;

                default:
                    yield return WaitForAnimation();
                    yield break;
            }
        }

        private IEnumerator JumpTo(Vector3 targetPosition)
        {
          float liftDelay = 0.10f;
            float animLen = _animatorController.GetAnimationLength();

            float totalMoveWindow = animLen * 0.5f;
            float moveDuration = Mathf.Max(0.01f, totalMoveWindow - liftDelay);
            
          float jumpPower = 1f;
          int numJumps = 1;

            _unitMover.JumpTo(
                _source.transform,
                targetPosition,
                duration: moveDuration,
                jumpPower: jumpPower,
                numJumps: numJumps,
                delay: liftDelay);

            yield return new WaitWhile(() => !_stopped && _unitMover.IsMoving);
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() => _animationPlaying = false;

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => !_stopped && _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }

        private static Vector3 CalculateTargetPosition(Vector3 start, Vector3 target)
        {
            Vector3 dir = (target - start).normalized;
            return target - dir * StopDistance;
        }

        private void FinishAbility()
        {
            _unitMover.Stop();
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }
    }
}
