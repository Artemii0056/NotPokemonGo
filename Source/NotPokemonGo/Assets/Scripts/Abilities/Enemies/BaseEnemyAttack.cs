using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Signals;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Enemies
{
    public sealed class BaseEnemyAttack : IAbilityHandler
    {
        private readonly ICoroutineRunner _runner;
        private readonly List<AbilityPart> _parts;

        private Unit _source;
        private Unit _target;

        private UnitAnimatorTrigger _trigger;
        private AnimatorController _anim;

        private Coroutine _routine;

        private bool _waitingFinishSignal;

        public BaseEnemyAttack(ICoroutineRunner runner, AbilityModel model)
        {
            _runner = runner;
            _parts = model.Parts;
            Interruptibility = model.Interruptibility;
        }

        public Interruptibility Interruptibility { get; }
        public event Action<IAbilityHandler> Finished;

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _trigger = source.AnimatorTrigger;
            _anim = source.AnimatorController;

            _routine = _runner.StartCoroutine(Run());
        }

        public void Stop()
        {
            if (_routine != null)
                _runner.StopCoroutine(_routine);

            Cleanup();
        }

        private IEnumerator Run()
        {
            // подписываемся один раз
            _anim.Signal += OnSignal;

            foreach (var part in _parts)
            {
                foreach (var phase in part.AbilityPhases)
                {
                    yield return PlayPhase(phase);
                }
            }

            Finish();
        }

        private IEnumerator PlayPhase(AbilityPhase phase)
        {
            _waitingFinishSignal = true;

            _trigger.SetTarget(_target);
            _trigger.SetPhase(phase);

            _anim.Play(phase.AnimationCashName);

            // 1) ждём PhaseSignal.Finish (99) из клипа
            yield return new WaitWhile(() => _waitingFinishSignal);

            // 2) Rule A: ждём, пока кирпичики фазы дозавершатся
            
        }

        private void OnSignal(int id)
        {
            // handler реагирует ТОЛЬКО на Finish
            if (id == (int)PhaseSignal.Finish)
                _waitingFinishSignal = false;
        }

        private void Finish()
        {
            Cleanup();
            _anim.Play(Constants.BaseAnimations.Idle);
            Finished?.Invoke(this);
        }

        private void Cleanup()
        {
            if (_anim != null)
                _anim.Signal -= OnSignal;

            _routine = null;
            _waitingFinishSignal = false;
        }
    }
}
