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

        private List<AbilityPhase> _phases;
        private Vector3 _startPosition;
        
        private bool _animationPlaying;

        public event Action Finished;

        public EngineeringSeriesAbility(Unit source, Unit target, IAbilityProvider abilityProvider, ICoroutineRunner coroutineRunner, IQteService qteService)
        {
            _target = target;
            _source = source;
            
            _coroutineRunner = coroutineRunner;
            _qteService = qteService;

            _unitAnimatorController = source.UnitAnimatorController;
            _unitAnimationTrigger = source.AnimatorTrigger;

            _phases = abilityProvider.AbilityModel.Phases;
            _startPosition = source.transform.position;
        }

        public void Play()
        {
            _coroutineRunner.StartCoroutine(HandlePhase(_phases));
        }

        private IEnumerator HandlePhase(List<AbilityPhase> phases)
        {
            foreach (AbilityPhase abilityPhase in phases)
            {
                _unitAnimationTrigger.SetPhase(abilityPhase);
                _unitAnimatorController.Play(abilityPhase.AnimationCashName);

                if (abilityPhase.QteType != QteType.Unknown) 
                    _qteService.Start(abilityPhase.QteType);

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

            _unitAnimatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke();
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