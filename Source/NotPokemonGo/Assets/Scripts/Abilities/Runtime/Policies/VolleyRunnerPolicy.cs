using System.Collections;
using Abilities.Configs;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments.Movers;
using QteSystem;
using Services;
using UnityEngine;

namespace Abilities.Runtime.Policies
{
    public class VolleyRunnerPolicy : AbilityPolicyBase
    {
        private readonly IShotImpactResolver _impactResolver;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ShotTracker _shotTracker = new();
        private readonly QteBinder _qteBinder;

        private AbilityContext _context;

        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        private Coroutine _coroutine;

        private int _currentCount;

        public VolleyRunnerPolicy(
            IQteService qteService,
           // IArmamentSpawner armamentSpawner,
           // IEffectsApplier effectsApplier,
            ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _qteBinder = new QteBinder(qteService);

            var policy = new ImpactPolicy
            {
                NoQteAction = ImpactAction.ApplyEffectsAndDestroy,
                OnFail = ImpactAction.ApplyEffectsAndDestroy,
                OnNormal = ImpactAction.DestroyOnly,
                OnPerfect = ImpactAction.ReflectToSourceAndDestroy
            };

            // _impactResolver = new DefaultShotImpactResolver(
            //     effectsApplier,
            //     armamentSpawner,
            //     policy,
            //     startShot: StartShot);
        }

        public override void OnAbilityStart(AbilityContext context)
        {
            if (context.Movers.Count == 0)
                return;

            _context = context;

            _coroutine = _coroutineRunner.StartCoroutine(Start());

            //Запуск корутиины? 

            // AbilityPhaseService phaseService = context?.AnimatorTrigger?.PhaseService;
            //
            // if (phaseService != null)
            //     phaseService.ArmamentRequested += OnArmamentRequested; 

            //Вот тут вопросики. По идее, нужно получить только старт и запустить корутину
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
        {
            _activePhase = phase;
            _finishSeenForActivePhase = false;
        }

        private IEnumerator Start()
        {
            while (_context.Movers.Count > 0)
            {
                IArmamentMover mover = _context.Movers[0];

                Shot reflected = new Shot(_activePhase, mover.Armament.Context, mover)
                {
                    RequiresQte = false
                };

                StartShot(reflected);

                Debug.Log("Start");

                yield return new WaitForSeconds(0.2f);
            }
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            // Debug.Log($"[FireballShotsPolicy] signal={signal} phaseMatch={ctx.CurrentPhase == _activePhase}");

            if (signal == PhaseSignal.Finish && ctx != null && ctx.CurrentPhase == _activePhase)
                _finishSeenForActivePhase = true;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            // var phaseService = ctx?.AnimatorTrigger?.PhaseService;
            //
            // if (phaseService != null)
            //     phaseService.ArmamentRequested -= OnArmamentRequested;

            _qteBinder.CleanupAll();
            _shotTracker.CleanupAll();

            _context = null;
            _activePhase = null;
            _finishSeenForActivePhase = false;
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;

            if (!PhaseHasArmament(phase))
                return true;

            if (!_finishSeenForActivePhase)
                return false;

            return _shotTracker.ActiveCount == 0 && _qteBinder.ActiveCount == 0;
        }

        private static bool PhaseHasArmament(AbilityPhase phase)
        {
            var actions = phase.SignalActions;

            if (actions == null)
                return false;

            for (int i = 0; i < actions.Count; i++)
            {
                PhaseSignalAction action = actions[i];

                if (action != null && action.HasArmament)
                    return true;
            }

            return false;
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
            Debug.Log(++_currentCount + " OnShotReached");
            
            _qteBinder.UnbindOnReach(shot);
            //_impactResolver.Resolve(shot);

            AbilityContext context = _context;

            if (context == null)
                return;

            AbilityPhase shotPhase = shot.Phase;

            if (shotPhase != null
                && shotPhase == _activePhase
                && _finishSeenForActivePhase)
            {
                if (_shotTracker.ActiveCount == 0 && _qteBinder.ActiveCount == 0)
                {
                    context.Animator?.FlagSignal((int)PhaseSignal.Finish);
                }
            }
        }
    }
}