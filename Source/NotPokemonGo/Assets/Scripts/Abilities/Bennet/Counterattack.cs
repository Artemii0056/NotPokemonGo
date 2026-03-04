using System;
using System.Collections;
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
    public sealed class Counterattack : IAbilityHandler
    {
        private readonly ICoroutineRunner _coroutineRunner;

        private AnimatorController _anim;
        private UnitAnimatorTrigger _trigger;

        private Unit _source;
        private Unit _target;

        private Coroutine _coroutine;
        private bool _waitingFinish;

        public event Action<IAbilityHandler> Finished;

        public Counterattack(ICoroutineRunner coroutineRunner, AbilityModel abilityModel)
        {
            _coroutineRunner = coroutineRunner;
            CurrentAbility = abilityModel;
        }

        public AbilityModel CurrentAbility { get; }

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

            if (_source == null)
            {
                FinishAbility();
                return;
            }

            _anim = _source.AnimatorController;
            _trigger = _source.AnimatorTrigger;

            if (_anim != null)
                _anim.Signal += OnAnimSignal;

            _coroutine = _coroutineRunner.StartCoroutine(ExecuteAllParts());
        }

        public void Stop()
        {
            if (_coroutine != null)
                _coroutineRunner.StopCoroutine(_coroutine);

            if (_anim != null)
                _anim.Signal -= OnAnimSignal;

            if (_source != null)
                _source.transform.DOKill();

            _waitingFinish = false;
            _coroutine = null;

            _source = null;
            _target = null;
            _anim = null;
            _trigger = null;
        }

        private IEnumerator ExecuteAllParts()
        {
            if (CurrentAbility?.Parts == null || CurrentAbility.Parts.Count == 0)
            {
                yield return ReturnToStart();
                FinishAbility();
                yield break;
            }

            for (int partIndex = 0; partIndex < CurrentAbility.Parts.Count; partIndex++)
            {
                var part = CurrentAbility.Parts[partIndex];
                if (part?.AbilityPhases == null) continue;

                for (int phaseIndex = 0; phaseIndex < part.AbilityPhases.Count; phaseIndex++)
                {
                    var phase = part.AbilityPhases[phaseIndex];
                    if (phase == null) continue;

                    yield return ExecutePhase(phase);
                }
            }

            yield return ReturnToStart();
            FinishAbility();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            if (_trigger != null)
            {
                _trigger.SetTarget(_target);
                _trigger.SetPhase(phase);
            }

            _waitingFinish = true;

            if (_anim != null)
                _anim.Play(phase.AnimationCashName);

            yield return new WaitWhile(() => _waitingFinish);

            if (_anim != null)
                _anim.Play(Constants.BaseAnimations.Idle);
        }

        private void OnAnimSignal(int id)
        {
            if (PhaseSignalUtil.FromInt(id) == PhaseSignal.Finish)
                _waitingFinish = false;
        }

        private IEnumerator ReturnToStart()
        {
            if (_source == null)
                yield break;

            Vector3 startPos = _source.StartPosition;

            const int jumpPower = 2;

            float duration = _source.AnimatorController != null
                ? Mathf.Max(0.05f, _source.AnimatorController.GetAnimationLength() * 0.5f)
                : 0.25f;

            _source.transform.DOKill();

            Tween tween = _source.transform
                .DOJump(startPos, jumpPower, 1, duration)
                .SetEase(Ease.InQuad);

            yield return tween.WaitForCompletion();

            if (_source.AnimatorController != null)
                _source.AnimatorController.Play(Constants.BaseAnimations.Idle);
        }

        private void FinishAbility() => Finished?.Invoke(this);
    }
}
