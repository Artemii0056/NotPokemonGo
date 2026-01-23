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
using UnityEngine;

namespace Abilities.Runtime
{
    public sealed class ComposedPhasedAbilityHandler : IAbilityHandler
    {
        private readonly ICoroutineRunner _runner;
        private readonly List<AbilityPart> _parts;
        private readonly List<IAbilityPolicy> _policies;

        private readonly AbilityContext _context = new();
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
            _context.Source = source;
            _context.Target = target;
            _context.AnimatorTrigger = source.AnimatorTrigger;
            _context.Animator = source.AnimatorController;

            if (_context.AnimatorTrigger != null)
                _context.AnimatorTrigger.PhaseService.BindFinishCheck(TryFinishPhase);

            foreach (IAbilityPolicy abilityPolicy in _policies)
                abilityPolicy.OnAbilityStart(_context);

            _context.Animator.Signal += OnAnimSignal;

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
            _context.CurrentPhase = phase;

            _context.AnimatorTrigger.SetTarget(_context.Target);
            _context.AnimatorTrigger.SetPhase(phase);

            foreach (IAbilityPolicy policy in _policies)
                policy.OnPhaseStart(_context, phase);

            _waitingPhaseFinish = true;
            _context.Animator.Play(phase.AnimationCashName);

            yield return new WaitWhile(() => _waitingPhaseFinish);
        }

        private void OnAnimSignal(int id)
        {
            var signal = PhaseSignalUtil.FromInt(id);

            if (signal == PhaseSignal.None)
                return;

            foreach (var policy in _policies)
                policy.OnSignal(_context, signal);

            TryFinishPhase();
        }

        private void TryFinishPhase()
        {
            if (_context.CurrentPhase == null)
                return;

            if (_policies.All(policy => policy.CanFinishPhase(_context, _context.CurrentPhase)))
                _waitingPhaseFinish = false;
        }

        private void OnAbilityFinished()
        {
            if (_context.Animator != null)
                _context.Animator.Play(Constants.BaseAnimations.Idle);
        }

        private void Cleanup()
        {
            if (_context.Animator != null)
                _context.Animator.Signal -= OnAnimSignal;

            if (_context.AnimatorTrigger != null)
                _context.AnimatorTrigger.PhaseService.BindFinishCheck(null);

            foreach (var policy in _policies)
                policy.OnAbilityStop(_context);

            _routine = null;
            _waitingPhaseFinish = false;
            _context.CurrentPhase = null;
        }
    }
}
