using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Runtime.Policies;
using Abilities.Signals;
using Services;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Runtime
{
    /// <summary>
    /// Эталонный AbilityHandler через композицию:
    /// - проигрывает Parts -> Phases
    /// - таймлайн = анимация
    /// - завершение фазы = AND по всем IAbilityPolicy.CanFinishPhase
    ///
    /// Любые особенности (QTE, живущие дольше фазы снаряды, особые правила stop и т.д.)
    /// добавляются политиками, без наследования.
    /// </summary>
    public sealed class ComposedPhasedAbilityHandler : IAbilityHandler
    {
        private readonly ICoroutineRunner _runner;
        private readonly List<AbilityPart> _parts;
        private readonly List<IAbilityPolicy> _policies;

        private readonly AbilityContext _ctx = new();
        private Coroutine _routine;
        private bool _waitingPhaseFinish;

        public Interruptibility Interruptibility { get; }
        public event Action<IAbilityHandler> Finished;

        public ComposedPhasedAbilityHandler(
            AbilityModel model,
            ICoroutineRunner runner,
            IEnumerable<IAbilityPolicy> policies)
        {
            _runner = runner;
            _parts = model.Parts;
            Interruptibility = model.Interruptibility;
            _policies = policies?.ToList() ?? new List<IAbilityPolicy>();
        }

        public void Play(Unit source, Unit target)
        {
            _ctx.Source = source;
            _ctx.Target = target;
            _ctx.AnimatorTrigger = source.AnimatorTrigger;
            _ctx.Animator = source.AnimatorController;

            foreach (var p in _policies)
                p.OnAbilityStart(_ctx);

            // Один подписчик на всю способность.
            _ctx.Animator.Signal += OnAnimSignal;

            _routine = _runner.StartCoroutine(RunAllParts());
        }

        public void Stop()
        {
            if (_routine != null)
                _runner.StopCoroutine(_routine);

            Cleanup();
        }

        private IEnumerator RunAllParts()
        {
            foreach (var part in _parts)
            {
                foreach (var phase in part.AbilityPhases)
                    yield return ExecutePhase(phase);
            }

            OnAbilityFinished();
            Finished?.Invoke(this);

            Cleanup();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase)
        {
            _ctx.CurrentPhase = phase;

            _ctx.AnimatorTrigger.SetTarget(_ctx.Target);
            _ctx.AnimatorTrigger.SetPhase(phase);

            foreach (var p in _policies)
                p.OnPhaseStart(_ctx, phase);

            _waitingPhaseFinish = true;
            _ctx.Animator.Play(phase.AnimationCashName);

            yield return new WaitWhile(() => _waitingPhaseFinish);
        }

        private void OnAnimSignal(int id)
        {
            var signal = PhaseSignalUtil.FromInt(id);
            if (signal == PhaseSignal.None)
                return;

            foreach (var p in _policies)
                p.OnSignal(_ctx, signal);

            TryFinishPhase();
        }

        private void TryFinishPhase()
        {
            if (_ctx.CurrentPhase == null)
                return;

            // AND по всем политикам.
            if (_policies.All(p => p.CanFinishPhase(_ctx, _ctx.CurrentPhase)))
                _waitingPhaseFinish = false;
        }

        private void OnAbilityFinished()
        {
            if (_ctx.Animator != null)
                _ctx.Animator.Play(Constants.BaseAnimations.Idle);
        }

        private void Cleanup()
        {
            if (_ctx.Animator != null)
                _ctx.Animator.Signal -= OnAnimSignal;

            foreach (var p in _policies)
                p.OnAbilityStop(_ctx);

            _routine = null;
            _waitingPhaseFinish = false;
            _ctx.CurrentPhase = null;
        }
    }
}
