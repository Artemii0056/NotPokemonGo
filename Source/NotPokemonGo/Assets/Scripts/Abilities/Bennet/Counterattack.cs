using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Signals;
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
        private AnimatorController _anim;
        private UnitAnimatorTrigger _trigger;
        
        private Unit _target;
        private Unit _source;

        private Coroutine _coroutine;

        private bool _waitingFinish;

        public event Action<IAbilityHandler> Finished;

        public Counterattack(
            ICoroutineRunner currentRoutine,
            AbilityModel abilityModel)
        {
            _coroutineRunner = currentRoutine;
            CurrentAbility = abilityModel;
        }
        
        public AbilityModel CurrentAbility { get; }

        public void Play(Unit source, Unit target) 
        {
            _source = source; 

            _target = target;

            _anim = _target.AnimatorController;
            _trigger = _target.AnimatorTrigger;
            _anim.Signal += OnAnimSignal;

            _coroutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());

            _source.HealthChanged += OnTargetHealthChanged;
        }

        public void Stop()
        {
            if (_coroutine != null)
                _coroutineRunner.StopCoroutine(_coroutine);

            _source.HealthChanged -= OnTargetHealthChanged;

            if (_anim != null)
                _anim.Signal -= OnAnimSignal;

            _source.transform.DOKill();

            _waitingFinish = false;
            _coroutine = null;
        }
        
        private void OnTargetHealthChanged(float arg1, float arg2)
        {
            _source.HealthChanged -= OnTargetHealthChanged;
            _coroutineRunner.StartCoroutine(PlayBackMove());
        }

        private IEnumerator ExecuteAllParts()
        {
            for (int partIndex = 0; partIndex < CurrentAbility.Parts.Count; partIndex++)
            {
                var part = CurrentAbility.Parts[partIndex];

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
            if (_anim != null)
                _anim.Signal -= OnAnimSignal;
            
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
            
           // _source.AnimatorController.Continue();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _trigger.SetTarget(_source);
            _trigger.SetPhase(phase);

            _waitingFinish = true;
            _anim.Play(phase.AnimationCashName);

            yield return new WaitWhile(() => _waitingFinish);

            _anim.Play(Constants.BaseAnimations.Idle);
        }

        private void OnAnimSignal(int id)
        {
            
            
            if (PhaseSignalUtil.FromInt(id) == PhaseSignal.Finish)
                _waitingFinish = false;
        }

        private void FinishAbility() => Finished?.Invoke(this);
    }
}