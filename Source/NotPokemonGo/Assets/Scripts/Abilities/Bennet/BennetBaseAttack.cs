using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.MV;
using DG.Tweening;
using Infrastructure;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Bennet
{
    public class BennetBaseAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly List<AbilityPart> _parts;
        
        private  UnitAnimatorController _animatorController;
        private  UnitAnimatorTrigger _animatorTrigger;
        private  Unit _source;
        private  Unit _target;
        private bool _animationPlaying;
        
        private Coroutine _currentRoutine;
        private  Vector3 _startPosition;

        public event Action<IAbilityHandler> Finished;

        public BennetBaseAttack(
            AbilityModel abilityModel,
            ICoroutineRunner coroutineRunner)
        {
            
            _coroutineRunner = coroutineRunner;
            
            _parts = abilityModel.Parts;
        }

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;
            
            _animatorController = source.UnitAnimatorController;
            _animatorTrigger = source.AnimatorTrigger;
            
            _startPosition = source.transform.position;

            _currentRoutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
        }

        public void Stop()
        {
            _coroutineRunner.StopCoroutine(_currentRoutine);
        }

        private IEnumerator ExecuteAllParts()
        {
            for (int partIndex = 0; partIndex < _parts.Count; partIndex++)
            {
                var part = _parts[partIndex];

                for (int phaseIndex = 0; phaseIndex < part.AbilityPhases.Count; phaseIndex++)
                {
                    var phase = part.AbilityPhases[phaseIndex];

                    yield return ExecutePhase(phase);
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
                    yield return MoveUnit(_source, CalculateTargetPosition(_source.transform.position, _target.transform.position));
                    break;
                
                case PhaseType.IsReturnPhase:
                    yield return MoveUnit(_source, _startPosition);
                    break;

                default:
                    yield return WaitForAnimation();
                    break;
            }
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() => FinishAnimation();

            _animatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _animatorController.Finished -= OnFinished;
        }
        
        private IEnumerator MoveUnit(Unit unit, Vector3 target)
        {
            float liftDelay = 0.1f;
            int jumpPower = 1;
            Debug.Log(_animatorController.GetAnimationName());

            yield return new WaitForSeconds(liftDelay);
            
            var duration = _animatorController.GetAnimationLength() / 2;

            float moveDuration = duration - liftDelay;
            unit.transform.DOKill();
            
            Tween jumpTween = unit.transform
                .DOJump(target, jumpPower, 1, moveDuration)
                .SetEase(Ease.InQuad);

            yield return jumpTween.WaitForCompletion();
        }
        
        private Vector3 CalculateTargetPosition(Vector3 start, Vector3 target)
        {
            float stopDistance = 1.5f;
            Vector3 direction = (target - start).normalized;
            return target - direction * stopDistance;
        }
        
        private void FinishAbility()
        {
            _animatorController.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }

        private void FinishAnimation() =>
            _animationPlaying = false;
    }
}