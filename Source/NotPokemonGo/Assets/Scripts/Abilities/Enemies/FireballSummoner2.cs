using System;
using System.Collections;
using System.Collections.Generic;
using Abilities.Bennet;
using Abilities.Configs;
using Abilities.General;
using Abilities.MV;
using Abilities.Runtime;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments;
using Armaments.Spawner;
using QTESystem;
using Services;
using Services.AbilityServices;
using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace Abilities.Enemies
{
    public sealed class FireballSummoner2 : IAbilityHandler
    {
        private readonly ICoroutineRunner _runner;
        private readonly IArmamentSpawner _armamentSpawner;

        private readonly ShotTracker _shotTracker = new();
        private readonly QteBinder _qteBinder;
        private readonly IShotImpactResolver _impactResolver;

        private readonly List<AbilityPart> _parts;

        public Interruptibility Interruptibility { get; }
        public event Action<IAbilityHandler> Finished;

        private Unit _source;
        private Unit _target;
        private UnitAnimatorTrigger _animTrigger;
        private AnimatorController _anim;

        private Coroutine _routine;

        // ожидание завершения фазы по сигналу Finish
        private bool _waitingFinishSignal;

        public FireballSummoner2(
            ICoroutineRunner runner,
            AbilityModel abilityModel,
            IQteService qteService,
            IArmamentSpawner armamentSpawner,
            IEffectsApplier effectsApplier)
        {
            _runner = runner;
            _armamentSpawner = armamentSpawner;

            _qteBinder = new QteBinder(qteService);

            Interruptibility = abilityModel.Interruptibility;
            _parts = abilityModel.Parts;

            var policy = new ImpactPolicy
            {
                NoQteAction = ImpactAction.ApplyEffectsAndDestroy,
                OnFail      = ImpactAction.ApplyEffectsAndDestroy,
                OnNormal    = ImpactAction.DestroyOnly,
                OnPerfect   = ImpactAction.ReflectToSourceAndDestroy
            };

            _impactResolver = new DefaultShotImpactResolver(
                effectsApplier,
                armamentSpawner,
                policy,
                startShot: StartShot);
        }

        public void Play(Unit source, Unit target)
        {
            _source = source;
            _target = target;

            _animTrigger = source.AnimatorTrigger;
            _anim = source.AnimatorController;

            // Подписка на сигналы анимации (для Finish)
            _anim.Signal += OnAnimSignal;

            // Подписка на armament из AbilityPhaseService
            //_source.AnimatorTrigger.AbilityPhaseService.ArmamentRequested += OnArmamentRequested;

            _routine = _runner.StartCoroutine(RunAbility());
        }

        public void Stop()
        {
            if (_routine != null)
                _runner.StopCoroutine(_routine);

            Cleanup();
        }

        private IEnumerator RunAbility()
        {
            foreach (var part in _parts)
                foreach (var phase in part.AbilityPhases)
                    yield return PlayPhase(phase);

            Finish();
        }

        private IEnumerator PlayPhase(AbilityPhase phase)
        {
            // 1) ставим контекст фазы
            _animTrigger.SetPhase(phase);
            _animTrigger.SetTarget(_target);

            // 2) готовимся ждать Finish-сигнал
            _waitingFinishSignal = true;

            // 3) играем клип
            _anim.Play(phase.AnimationCashName);

            // 4) ждём пока клип пришлёт FlagSignal(Finish)
            yield return new WaitWhile(() => _waitingFinishSignal);
        }

        private void OnAnimSignal(int id)
        {
            if (id == (int)PhaseSignal.Finish)
                _waitingFinishSignal = false;
        }

        private void OnArmamentRequested(ArmamentRequest req)
        {
            // Канон: фаерболы могут быть мульти-таргет — спавним по каждому
            foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(req))
            {
                var mover = _armamentSpawner.Create(ctx);

                var shot = new Shot(req.Phase, ctx, mover)
                {
                    RequiresQte = req.Phase.QteType != QteType.Unknown
                };

                StartShot(shot);
            }
        }

        private void StartShot(Shot shot)
        {
            _shotTracker.Register(shot, OnShotLaunched, OnShotReached);
            shot.Mover.Move();
        }

        private void OnShotLaunched(Shot shot)
        {
            _qteBinder.BindOnLaunch(
                shot,
                qteTarget: shot.Context.Target,
                timeoutPolicy: QteTimeoutPolicy.TreatAsFail);
        }

        private void OnShotReached(Shot shot)
        {
            _qteBinder.UnbindOnReach(shot);
            _impactResolver.Resolve(shot);
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
                _anim.Signal -= OnAnimSignal;

            // if (_source?.AnimatorTrigger != null)
            //     _source.AnimatorTrigger.AbilityPhaseService.ArmamentRequested -= OnArmamentRequested;

            _qteBinder.CleanupAll();
            _shotTracker.CleanupAll();

            _routine = null;
            _waitingFinishSignal = false;
        }
    }
}
