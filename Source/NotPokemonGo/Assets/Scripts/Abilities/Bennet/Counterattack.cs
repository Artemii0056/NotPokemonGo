using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.MV;
using DG.Tweening;
using Infrastructure;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Bennet
{
    public class Counterattack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private UnitAnimatorController _animatorController;

        private UnitAnimatorTrigger _animatorTrigger;

        private readonly List<AbilityPart> _parts;
        private readonly ITargetSelector _targetSelector;
        private ISourceProvider _sourceProvider;

        private Unit _target;
        private Unit _source;

        private Coroutine _coroutine;

        private bool _animationPlaying;

        public event Action<IAbilityHandler> Finished;

        public Counterattack(
            ICoroutineRunner currentRoutine,
            AbilityModel abilityModel,
            ITargetSelector targetSelector,
            ISourceProvider sourceProvider)
        {
            _coroutineRunner = currentRoutine;
            _parts = abilityModel.Parts;
            _targetSelector = targetSelector;
            _sourceProvider = sourceProvider;
        }

        public void Play()
        {
            _source = _sourceProvider.Source; 

            _target = _targetSelector.Target;
            _targetSelector.Remember(_source);

            _source.UnitAnimatorController.Pause();

            _coroutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());

            _source.HealthChanged += OnTargetHealthChanged;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void OnTargetHealthChanged(float arg1, float arg2)
        {
            _source.HealthChanged -= OnTargetHealthChanged;
            _coroutineRunner.StartCoroutine(PlayBackMove());
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
        }

        private IEnumerator PlayBackMove()
        {
            yield return MoveUnit(_source, _source.StartPosition);

            _source.UnitAnimatorController.Play(Constants.BaseAnimations.Idle);
            
            FinishAbility();
        }

        private IEnumerator MoveUnit(Unit source, Vector3 sourceStartPosition)
        {
            int jumpPower = 2;
            var duration = source.UnitAnimatorController.GetAnimationLength();

            float moveDuration = duration /2;

            source.transform.DOKill();

            Tween jumpTween = source.transform
                .DOJump(sourceStartPosition, jumpPower, 1, moveDuration)
                .SetEase(Ease.InQuad);

            yield return jumpTween.WaitForCompletion();
            _source.UnitAnimatorController.Continue();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _target.AnimatorTrigger.SetPhase(phase);
            _target.UnitAnimatorController.Play(phase.AnimationCashName);

            yield return WaitForAnimation();
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() => FinishAnimation();

            _target.UnitAnimatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _target.UnitAnimatorController.Finished -= OnFinished;

            _target.UnitAnimatorController.Play(Constants.BaseAnimations.Idle); 
        }

        private void FinishAbility() => 
            Finished?.Invoke(this);

        private void FinishAnimation() =>
            _animationPlaying = false;
    }
}