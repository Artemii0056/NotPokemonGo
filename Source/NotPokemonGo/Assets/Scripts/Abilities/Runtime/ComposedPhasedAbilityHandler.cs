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
        private readonly List<IAbilityPolicy> _policies;

        private readonly AbilityContext _context = new();
        private Coroutine _routine;
        private bool _waitingPhaseFinish;

        public event Action<IAbilityHandler> Finished;

        public ComposedPhasedAbilityHandler(
            AbilityModel abilityModel,
            ICoroutineRunner runner,
            IEnumerable<IAbilityPolicy> policies)
        {
            CurrentAbility = abilityModel ?? throw new ArgumentNullException(nameof(abilityModel));
            _runner = runner ?? throw new ArgumentNullException(nameof(runner));
            _policies = policies?.ToList() ?? new List<IAbilityPolicy>();
        }

        public AbilityModel CurrentAbility { get; }

        public void Play(Unit source, Unit target)
        {
            _context.Source = source;
            _context.Target = target;
            _context.AnimatorTrigger = source.AnimatorTrigger;
            _context.Animator = source.AnimatorController;

            if (_context.AnimatorTrigger != null)
                _context.AnimatorTrigger.PhaseService.BindFinishCheck(TryFinishPhase);

            foreach (var policy in _policies)
                policy.OnAbilityStart(_context);

            if (_context.Animator != null)
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
            foreach (var part in CurrentAbility.Parts)
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

            _context.AnimatorTrigger.PhaseService.BeginPhase(phase);

            foreach (var policy in _policies)
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

            _context.AnimatorTrigger?.PhaseService.RequestFinishCheck();
        }

        private void TryFinishPhase()
        {
            if (_context.CurrentPhase == null) 
                return;

            bool can = _policies.All(p => p.CanFinishPhase(_context, _context.CurrentPhase));
            
            Debug.Log($"[TryFinishPhase] phase={_context.CurrentPhase.AnimationCashName} can={can}");

            if (can)
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
