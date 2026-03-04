using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.MV;
using Abilities.Runtime.Policies;
using Abilities.Signals;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Services;
using Units;
using UnityEngine;

namespace Abilities.Runtime
{
    public sealed class ComposedPhasedAbilityHandler : IAbilityHandler
    {
        private static int _nextRunId;

        private readonly ICoroutineRunner _runner;
        private readonly List<IAbilityPolicy> _policies;

        private readonly AbilityContext _context = new();

        private Coroutine _routine;
        private List<IAbilityPolicy> _activePolicies;

        private CancellationTokenSource _cts;

        private int _runId;

        public event Action<IAbilityHandler> Finished;

        public ComposedPhasedAbilityHandler(
            AbilityModel abilityModel,
            ICoroutineRunner runner,
            IEnumerable<IAbilityPolicy> policies)
        {
            CurrentAbility = abilityModel ?? throw new ArgumentNullException(nameof(abilityModel));
            _runner = runner ?? throw new ArgumentNullException(nameof(runner));
            _policies = policies?.ToList() ?? new List<IAbilityPolicy>();
            _activePolicies = new List<IAbilityPolicy>();
        }

        public AbilityModel CurrentAbility { get; }

        public void Play(Unit source, Unit target)
        {
            Stop();

            // Unique owner id for DOTween + PhaseGate ownership within this ability run.
            _runId = unchecked(++_nextRunId);

            _cts = new CancellationTokenSource();

            _context.Source = source;
            _context.Target = target;
            _context.AnimatorTrigger = source.AnimatorTrigger;
            _context.Animator = source.AnimatorController;

            foreach (var policy in _policies)
                policy.OnAbilityStart(_context);

            if (_context.Animator != null)
                _context.Animator.Signal += OnAnimSignal;

            _routine = _runner.StartCoroutine(RunAllParts(_cts.Token));
        }

        public void Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            // Kill all tweens created under this ability run id.
            if (_runId != 0)
                DOTween.Kill(_runId);

            if (_routine != null)
            {
                _runner.StopCoroutine(_routine);
                _routine = null;
            }

            Cleanup();
        }

        private IEnumerator RunAllParts(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Cleanup();
                yield break;
            }

            foreach (var part in CurrentAbility.Parts)
            {
                foreach (var phase in part.AbilityPhases)
                {
                    if (token.IsCancellationRequested)
                    {
                        Cleanup();
                        yield break;
                    }

                    yield return ExecutePhase(phase, token);
                }
            }

            OnAbilityFinished();
            Finished?.Invoke(this);
            Cleanup();
        }

        private IEnumerator ExecutePhase(AbilityPhase phase, CancellationToken token)
        {
            _activePolicies = new List<IAbilityPolicy>();

            _context.CurrentPhase = phase;

            var trigger = _context.AnimatorTrigger;
            var animator = _context.Animator;

            if (trigger == null || animator == null || phase == null)
                yield break;

            trigger.SetTarget(_context.Target);
            trigger.SetPhase(phase);

            var phaseService = trigger.PhaseService;
            phaseService.BeginPhase(phase, _runId);

            for (int i = 0; i < _policies.Count; i++)
            {
                var policy = _policies[i];
                if (!policy.CanUseAbility(_context))
                    continue;

                _activePolicies.Add(policy);
                policy.OnPhaseStart(_context, phase);
            }

            animator.Play(phase.AnimationCashName);

            // Deterministic completion: wait for Finish signal AND PhaseGate opened.
            // Safety net: if Finish event is missing (common for movement/loop phases), force finish after timeout.
            yield return WaitPhaseWithWatchdog(phaseService, phase, token).ToCoroutine();
        }

        private static UniTask WaitPhaseWithWatchdog(
            Services.AbilityServices.AbilityPhaseService phaseService,
            AbilityPhase phase,
            CancellationToken token)
        {
            // If clip is missing, use a conservative fallback.
            float clipLen = phase?.AnimationClip != null ? phase.AnimationClip.length : 0.5f;
            float timeoutSec = Mathf.Clamp(clipLen + 0.25f, 0.25f, 10f);

            return WaitPhaseWithWatchdogInternal(phaseService, timeoutSec, token);
        }

        private static async UniTask WaitPhaseWithWatchdogInternal(
            Services.AbilityServices.AbilityPhaseService phaseService,
            float timeoutSec,
            CancellationToken token)
        {
            // Fast path
            UniTask waitTask = phaseService.WaitPhaseCompletionAsync(token);
            UniTask delayTask = UniTask.Delay(TimeSpan.FromSeconds(timeoutSec), cancellationToken: token);

            int winner = await UniTask.WhenAny(waitTask, delayTask);
            if (winner == 0)
                return;

            // Timeout -> dump holders and force finish.
            phaseService.DumpGateDebug($"[Ability] Phase watchdog timeout ({timeoutSec:0.00}s)");
            phaseService.ForceFinish($"Watchdog timeout {timeoutSec:0.00}s");

            // Await completion after forcing.
            await phaseService.WaitPhaseCompletionAsync(token);
        }

        private void OnAnimSignal(int id)
        {
            var signal = PhaseSignalUtil.FromInt(id);
            if (signal == PhaseSignal.None)
                return;

            for (int i = 0; i < _activePolicies.Count; i++)
                _activePolicies[i].OnSignal(_context, signal);
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

            if (_activePolicies != null)
            {
                for (int i = 0; i < _activePolicies.Count; i++)
                    _activePolicies[i].OnAbilityStop(_context);

                _activePolicies.Clear();
            }

            _context.CurrentPhase = null;
        }
    }
}
