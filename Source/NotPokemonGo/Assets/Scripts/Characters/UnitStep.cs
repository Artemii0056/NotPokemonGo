using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Characters
{
    public class UnitStep
    {
        private readonly UnitAnimatorTrigger _unitAnimatorTrigger;
        private readonly UnitAnimatorController _unitAnimatorController;
        private readonly ICoroutineRunner _coroutineRunner;

        private bool _animationPlaying;

        private Vector3 _startPosition;

        private List<AbilityPhase> _phases;
        private Unit _source;
        private Unit _target;

        public UnitStep(
            UnitAnimatorTrigger unitAnimatorTrigger,
            UnitAnimatorController unitAnimatorController,
            ICoroutineRunner coroutineRunner)
        {
            _unitAnimatorTrigger = unitAnimatorTrigger;
            _unitAnimatorController = unitAnimatorController;
            _coroutineRunner = coroutineRunner;
        }

        public event Action ActionEnded;

        public void SetAbilityModel(AbilityModel abilityModel, Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _phases = abilityModel.Phases;

            _startPosition = source.transform.position;

            _coroutineRunner.StartCoroutine(HandlePhase(_phases));
        }

        private IEnumerator HandlePhase(List<AbilityPhase> phases)
        {
            foreach (AbilityPhase abilityPhase in phases)
            {
                _unitAnimatorTrigger.SetPhase(abilityPhase);
                _unitAnimatorController.Play(abilityPhase.AnimationCashName);

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
            ActionEnded?.Invoke();
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