using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.AbilitySteps;
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

        public event Action<IAbilityHandler> Finished;

        public BennetBaseAttack(ICoroutineRunner coroutineRunner, AbilityModel abilityModel)
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

                    // 🔸 пример: пропустить фазы после неудачного QTE
                    // if (!_lastQteSuccess && phase.QteType == QteType.SliderForward)
                    // {
                    //     Debug.Log($"Фаза {phaseIndex} пропущена из-за неудачного QTE");
                    //     continue;
                    // }

                    yield return ExecutePhase(phase);
                }
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
                    yield return MoveUnit(_source, CalculateTargetPosition(_source.transform.position, _target.transform.position));
                    break;
                
                case PhaseType.IsReturnPhase:
                    yield return MoveUnit(_source, _source.StartPosition);
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
            float liftDelay = 0.6f;
            int jumpPower = 2;
            var duration = _animatorController.GetAnimationLength();

            yield return new WaitForSeconds(liftDelay);

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