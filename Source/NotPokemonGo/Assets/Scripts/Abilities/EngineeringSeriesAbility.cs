using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using QTESystem;
using Services;
using Services.QTEServices;
using Units;
using Units.AnimationControllers;
using UnityEngine;
using DG.Tweening;

namespace Abilities
{
    public class EngineeringSeriesAbility
    {
        private Unit _source;
        private Unit _target;

        private UnitAnimatorTrigger _unitAnimationTrigger;
        private UnitAnimatorController _unitAnimatorController;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IQteService _qteService;

        private int _currentPhaseIndex;

        private List<AbilityPart> _parts;
        private Vector3 _startPosition;

        private bool _animationPlaying;

        private Coroutine _coroutine;

        private bool _tapCompleted;
        private bool _backCompleted;
        private bool _forwardCompleted;

        public event Action Finished;

        public EngineeringSeriesAbility(
            Unit source,
            Unit target,
            IAbilityProvider abilityProvider,
            ICoroutineRunner coroutineRunner,
            IQteService qteService)
        {
            _target = target;
            _source = source;

            _coroutineRunner = coroutineRunner;
            _qteService = qteService;

            _unitAnimatorController = source.UnitAnimatorController;
            _unitAnimationTrigger = source.AnimatorTrigger;

            _parts = abilityProvider.AbilityModel.Parts;
            _startPosition = source.transform.position;
        }

        public void Play()
        {
            _currentPhaseIndex = 0;

            _coroutine = _coroutineRunner.StartCoroutine(HandlePhase(_parts[_currentPhaseIndex].AbilityPhases));
        }

        private IEnumerator HandlePhase(List<AbilityPhase> phases)
        {
            foreach (AbilityPhase abilityPhase in phases)
            {
                _unitAnimationTrigger.SetPhase(abilityPhase);
                _unitAnimatorController.Play(abilityPhase.AnimationCashName);

                if (abilityPhase.QteType != QteType.Unknown)
                {
                    if (abilityPhase.QteType == QteType.TapToButton)
                    {
                        _qteService.Completed += OnQteTapToButtonCompleted;
                        _qteService.Start(abilityPhase.QteType);
                    }
                    else if (abilityPhase.QteType == QteType.SliderBack)
                    {
                        if (_tapCompleted)
                        {
                            _qteService.Completed += OnQteSliderBackCompleted;
                            _qteService.Start(abilityPhase.QteType);
                        }
                    }
                    else if (abilityPhase.QteType == QteType.SliderForward)
                    {
                        if (_backCompleted)
                        {
                            _qteService.Completed += OnQteSliderForwardCompleted;
                            _qteService.Start(abilityPhase.QteType);
                        }
                    }
                }

                switch (abilityPhase.PhaseType)
                {
                    case PhaseType.IsMelee:
                        _animationPlaying = true;
                        _unitAnimatorController.Finished += AnimationFinished;

                        yield return new WaitWhile(() => _animationPlaying);

                        _unitAnimatorController.Finished -= AnimationFinished;
                        break;

                    case PhaseType.IsMovementPhase:
                        yield return MoveUnit(_source, _target.transform.position, 1f);
                        break;

                    case PhaseType.IsReturnPhase:
                        yield return MoveUnit(_source, _startPosition);
                        break;

                    case PhaseType.Default:
                        _animationPlaying = true;
                        _unitAnimatorController.Finished += AnimationFinished;

                        yield return new WaitWhile(() => _animationPlaying);

                        _unitAnimatorController.Finished -= AnimationFinished;
                        break;
                }
            }

            HandleEndPhase();
        }

        private void OnQteSliderBackCompleted(bool state)
        {
            Debug.Log("OnQteSliderBackCompleted " + state);

            _backCompleted = state;
            _qteService.Completed -= OnQteSliderBackCompleted;
        }

        private void OnQteSliderForwardCompleted(bool state)
        {
            Debug.Log("OnQteSliderForwardCompleted " + state);

            Debug.Log("ПРОЙДЕНО!");
            _qteService.Completed -= OnQteSliderForwardCompleted;
        }

        private void OnQteTapToButtonCompleted(bool state)
        {
            Debug.Log("OnQteTapToButtonCompleted " + state);

            _tapCompleted = state;

            _qteService.Completed -= OnQteTapToButtonCompleted;

            // if (state == false)
            //     _currentPhaseIndex++;
        }

        private void HandleEndPhase()
        {
            _currentPhaseIndex++;

            if (_currentPhaseIndex >= _parts.Count)
            {
                _unitAnimatorController.Play(Constants.BaseAnimations.Idle);
                Finished?.Invoke();
                return;
            }

            if (_coroutine != null)
                _coroutineRunner.StopCoroutine(_coroutine);

            _coroutine = _coroutineRunner.StartCoroutine(HandlePhase(_parts[_currentPhaseIndex].AbilityPhases));
        }

        private void AnimationFinished() =>
            _animationPlaying = false;

        private IEnumerator MoveUnit(Unit unit, Vector3 targetPosition, float offset = 0)
        {
            float stopDistance = 1.5f;
            float liftDelay = 0.6f;
            int jumpPower = 2;
            var totalDuration = _unitAnimatorController.GetAnimationLenght();

            Vector3 direction = (targetPosition - unit.transform.position).normalized;
            Vector3 adjustedTarget = targetPosition - direction * stopDistance;

            yield return new WaitForSeconds(liftDelay);

            float moveDuration = totalDuration - liftDelay;
            unit.transform.DOKill();

            Tween jumpTween = unit.transform
                .DOJump(adjustedTarget, jumpPower, 1, moveDuration)
                .SetEase(Ease.InQuad);

            yield return jumpTween.WaitForCompletion();
        }
    }
}