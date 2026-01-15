using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Signals;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Bennet
{
    public sealed class BennetBaseAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _runner;
        private readonly List<AbilityPart> _parts;

        private Unit _source;
        private Unit _target;
        private UnitAnimatorTrigger _trigger;
        private AnimatorController _anim;

        private Coroutine _routine;

        private bool _waitingFinish;
        private Action<int> _onSignal;

        public event Action<IAbilityHandler> Finished;
        public Interruptibility Interruptibility { get; }

        public BennetBaseAttack(AbilityModel model, ICoroutineRunner runner)
        {
            _runner = runner;
            _parts = model.Parts;
            Interruptibility = model.Interruptibility;
        }

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _trigger = source.AnimatorTrigger;
            _anim = source.AnimatorController;

            source.SetStartPosition(source.transform.position);

            _routine = _runner.StartCoroutine(Run());
        }

        public void Stop()
        {
            if (_routine != null)
                _runner.StopCoroutine(_routine);

            UnbindFinishWait();
            _routine = null;
        }

        private IEnumerator Run()
        {
            foreach (var part in _parts)
            {
                foreach (var phase in part.AbilityPhases)
                    yield return PlayPhase(phase);
            }

            _anim.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }

        private IEnumerator PlayPhase(AbilityPhase phase)
        {
            _trigger.SetTarget(_target);
            _trigger.SetPhase(phase);

            BindFinishWait();
            _anim.Play(phase.AnimationCashName);

            yield return new WaitWhile(() => _waitingFinish);

            UnbindFinishWait();
        }

        private void BindFinishWait()
        {
            _waitingFinish = true;
            _onSignal = id =>
            {
                if (PhaseSignalUtil.FromInt(id) == PhaseSignal.Finish)
                    _waitingFinish = false;
            };
            _anim.Signal += _onSignal;
        }

        private void UnbindFinishWait()
        {
            if (_onSignal != null && _anim != null)
                _anim.Signal -= _onSignal;

            _onSignal = null;
            _waitingFinish = false;
        }
    }
}
