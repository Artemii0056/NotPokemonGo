using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Services;
using Units;
using Units.AnimationControllers;
using Units.Movement;
using UnityEngine;

namespace Abilities.Enemies
{
    public sealed class BaseEnemyAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly UnitMover _unitMover;
        private readonly List<AbilityPart> _parts;

        private Unit _source;
        private Unit _target;

        private UnitAnimatorTrigger _animatorTrigger;
        private AnimatorController _animatorController;

        private Coroutine _routine;
        private bool _animationPlaying;
        private bool _stopped;

        private Vector3 _startPosition;

        private const float MoveSpeed = 4f;
        private const float StopDistance = 1.5f;

        public BaseEnemyAttack(
            ICoroutineRunner coroutineRunner,
            AbilityModel abilityModel,
            UnitMover unitMover)
        {
            _coroutineRunner = coroutineRunner;
            _unitMover = unitMover;
            _parts = abilityModel.Parts;
            Interruptibility = abilityModel.Interruptibility;
        }

        public Interruptibility Interruptibility { get; }
        public event Action<IAbilityHandler> Finished;

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _animatorTrigger = source.AnimatorTrigger;
            _animatorController = source.AnimatorController;

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

            Finish();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _animatorTrigger.SetTarget(_target);
            _animatorTrigger.SetPhase(phase);
            _animatorController.Play(phase.AnimationCashName);

            switch (phase.PhaseType)
            {
                case PhaseType.IsMovementPhase:
                    yield return MoveTo(CalculateApproachPoint());
                    break;

                case PhaseType.IsReturnPhase:
                    yield return MoveTo(_startPosition);
                    break;

                default:
                    yield return WaitForAnimation();
                    break;
            }
        }

        private IEnumerator MoveTo(Vector3 targetPosition)
        {
            _unitMover.MoveTo(_source.transform, targetPosition, MoveSpeed);

            yield return new WaitWhile(() =>
                !_stopped && _unitMover.IsMoving
            );
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() => _animationPlaying = false;

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => !_stopped && _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }

        private Vector3 CalculateApproachPoint()
        {
            Vector3 start = _source.transform.position;
            Vector3 target = _target.transform.position;

            Vector3 dir = (target - start).normalized;
            return target - dir * StopDistance;
        }

        private void Finish()
        {
            _unitMover.Stop();
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }
    }
}
