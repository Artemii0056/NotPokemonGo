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
    public class Counterattack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private AnimatorController _animatorController;

        private UnitAnimatorTrigger _animatorTrigger;

        private readonly List<AbilityPart> _parts;

        private Unit _target;
        private Unit _source;

        private Coroutine _coroutine;

        private bool _animationPlaying;

        public event Action<IAbilityHandler> Finished;
        public Interruptibility Interruptibility { get; }

        public Counterattack(
            ICoroutineRunner currentRoutine,
            AbilityModel abilityModel)
        {
            _coroutineRunner = currentRoutine;
            _parts = abilityModel.Parts;
            
            Interruptibility = abilityModel.Interruptibility;
        }

        public void Play(Unit source, Unit target) 
        {
            _source = source; 

            _target = target;

            _source.AnimatorController.Pause();

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

            _source.AnimatorController.Play(Constants.BaseAnimations.Idle);
            
            FinishAbility();
        }

        private IEnumerator MoveUnit(Unit source, Vector3 sourceStartPosition)
        {
            int jumpPower = 2;
            var duration = source.AnimatorController.GetAnimationLength();

            float moveDuration = duration /2;

            source.transform.DOKill();

            Tween jumpTween = source.transform
                .DOJump(sourceStartPosition, jumpPower, 1, moveDuration)
                .SetEase(Ease.InQuad);

            yield return jumpTween.WaitForCompletion();
            
            _source.AnimatorController.Continue();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _target.AnimatorTrigger.SetTarget(_source);
            _target.AnimatorTrigger.SetPhase(phase);
            _target.AnimatorController.Play(phase.AnimationCashName);

            yield return WaitForAnimation();
        }

        private IEnumerator WaitForAnimation()
        {
            _animationPlaying = true;

            void OnFinished() => FinishAnimation();

            _target.AnimatorController.Finished += OnFinished;
            yield return new WaitWhile(() => _animationPlaying);
            _target.AnimatorController.Finished -= OnFinished;

            _target.AnimatorController.Play(Constants.BaseAnimations.Idle); 
        }

        private void FinishAbility() => 
            Finished?.Invoke(this);

        private void FinishAnimation() =>
            _animationPlaying = false;
    }
}