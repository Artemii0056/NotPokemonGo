using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.AbilitySteps;
using Abilities.Bennet;
using Abilities.MV;
using Infrastructure;
using Services;
using Services.AbilityServices;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Enemies
{
    public class BaseEnemyAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private UnitAnimatorTrigger _animatorTrigger;
        private UnitAnimatorController _animatorController;

        private bool _animationPlaying;

        private Vector3 _startPosition;

        private readonly List<AbilityStepData> _steps;
        private Unit _source;
        private Unit _target;

        private Coroutine _currentRoutine;

        public event Action<IAbilityHandler> Finished;

        public BaseEnemyAttack(
            ICoroutineRunner coroutineRunner,
            AbilityModel abilityModel)
        {
            _coroutineRunner = coroutineRunner;
            _steps = abilityModel.Steps;
        }
        
        public void Play(Unit source, Unit target)
        {
            _target = target;
            _source = source;

            _animatorTrigger = source.AnimatorTrigger;
            _animatorController = source.UnitAnimatorController;

            _startPosition = _source.transform.position;
            _currentRoutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
        }

        public void Stop()
        {
            _coroutineRunner.StopCoroutine(_currentRoutine);
        }

        private IEnumerator ExecuteAllParts()
        {
            for (int partIndex = 0; partIndex < _steps.Count; partIndex++)
            {
                AbilityStepData abilitStepData = _steps[partIndex];
                yield return ExecutePhase(abilitStepData);
            }

            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityStepData abilityStepData)
        {
            _animatorTrigger.SetTarget(_target);
            _animatorTrigger.SetPhase(abilityStepData);
            _animatorController.Play(abilityStepData.AnimationCashName);

            switch (abilityStepData.PhaseType)
            {
                case PhaseType.IsMovementPhase:
                    yield return MoveUnit(_source,
                        CalculateTargetPosition(_source.transform.position, _target.transform.position));
                    break;

                case PhaseType.IsReturnPhase:
                    yield return MoveUnit(_source, _startPosition);
                    break;

                default:
                    yield return WaitForAnimation();
                    break;
            }
        }

        private void FinishAbility()
        {
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() =>
                FinishAnimation();

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }

        private Vector3 CalculateTargetPosition(Vector3 start, Vector3 target)
        {
            float stopDistance = 1.5f;
            Vector3 direction = (target - start).normalized;
            return target - direction * stopDistance;
        }

        private void FinishAnimation() =>
            _animationPlaying = false;

        private IEnumerator MoveUnit(Unit unit, Vector3 targetPosition, float offset = 0)
        {
            const float Speed = 4f;

            while (Vector3.Distance(unit.transform.position, targetPosition) > offset)
            {
                unit.transform.position = Vector3.MoveTowards(
                    unit.transform.position,
                    targetPosition,
                    Speed * Time.deltaTime);

                yield return null;
            }
        }
    }
}