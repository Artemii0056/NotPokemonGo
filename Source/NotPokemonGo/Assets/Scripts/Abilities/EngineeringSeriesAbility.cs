using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using QTESystem;
using Services;
using Services.QTEServices;
using Units;
using Units.AnimationControllers;
using Unity.Collections;
using UnityEngine;

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
                    }
                    else if (abilityPhase.QteType == QteType.SliderPingPong)
                    {
                        _qteService.Completed += OnQteSliderPingPongCompleted;
                    }

                    _qteService.Start(abilityPhase.QteType);
                    //_qteService.Completed += OnQteTapToButtonCompleted;
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

        private void OnQteSliderPingPongCompleted(bool state)
        {
            throw new NotImplementedException();
        }

        private void OnQteTapToButtonCompleted(bool state)
        {
            _qteService.Completed -= OnQteTapToButtonCompleted;
            
                Debug.Log("12321");
                
            if (state == false)
                _currentPhaseIndex++;
            
                Debug.Log(state);
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
            {
                _coroutineRunner.StopCoroutine(_coroutine);
            }

            _coroutine = _coroutineRunner.StartCoroutine(HandlePhase(_parts[_currentPhaseIndex].AbilityPhases));
        }

        private void AnimationFinished() =>
            _animationPlaying = false;

        private IEnumerator MoveUnit(Unit unit, Vector3 targetPosition, float offset = 0)
        {
            const float Speed = 2f;

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