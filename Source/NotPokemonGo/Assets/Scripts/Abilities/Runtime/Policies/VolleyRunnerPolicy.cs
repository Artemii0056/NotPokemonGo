using System.Collections;
using Abilities.Configs;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments;
using Armaments.Movers;
using Effects;
using Platoons;
using QteSystem;
using Services;
using Services.StaticDataServices;
using Spawners.Spawner;
using Statuses;
using Statuses.Services;
using UnityEngine;

namespace Abilities.Runtime.Policies
{
    public class VolleyRunnerPolicy : AbilityPolicyBase
    {
        private readonly IShotImpactResolver _impactResolver;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStatusFactory _statusFactory;
        private readonly IQteService _qteService;
        private readonly IStaticDataService _staticDataService;
        private readonly IStatusResolver _statusResolver;
        private readonly ShotTracker _shotTracker = new();

        private AbilityContext _context;

        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        private Coroutine _coroutine;

        private int _currentCount;

        private IQteSession _qteSession;

        public VolleyRunnerPolicy(
            IQteService qteService,
            ICoroutineRunner coroutineRunner,
            IEffectsApplier effectsApplier,
            IArmamentSpawner armamentSpawner,
            IStaticDataService staticDataService, IStatusFactory statusFactory, IStatusResolver statusResolver)
        {
            _qteService = qteService;
            _coroutineRunner = coroutineRunner;
            _staticDataService = staticDataService;
            _statusFactory = statusFactory;
            _statusResolver = statusResolver;

            _impactResolver = new DefaultShotImpactResolver(effectsApplier, armamentSpawner, null, StartShot);
        }

        public override bool CanUseAbility(AbilityContext ctx) =>
            ctx.Movers.Count > 0;

        public override void OnAbilityStart(AbilityContext context)
        {
            if (context.Movers.Count == 0)
                return;

            _context = context;
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
        {
            _activePhase = phase;
            _finishSeenForActivePhase = false;

            _context = ctx;

            _coroutine = _coroutineRunner.StartCoroutine(Start());
        }

        private IEnumerator Start()
        {
            _qteSession = _qteService.StartSession(_context.CurrentPhase.QteType, _context.Target, 100);

            while (_context.Movers.Count > 0)
            {
                IArmamentMover mover = _context.Movers[0];

                Shot reflected = new Shot(_activePhase, mover.Armament.Context, mover)
                {
                    RequiresQte = false
                };

                _context.Movers.Remove(mover);
                StartShot(reflected);

                yield return new WaitForSeconds(0.3f);
            }

            //_qteSession.Dispose();
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            if (signal == PhaseSignal.Finish && ctx != null && ctx.CurrentPhase == _activePhase)
                _finishSeenForActivePhase = true;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            _shotTracker.CleanupAll();

            _context = null;
            _activePhase = null;
            _finishSeenForActivePhase = false;
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;

            if (!_finishSeenForActivePhase)
                return false;

            bool canFinishPhase = _shotTracker.ActiveCount == 0 &&
                                 _currentCount >= 3;

            if (canFinishPhase) 
                _qteSession.Dispose();
            
            return canFinishPhase;
        }

        private void StartShot(Shot shot)
        {
            _shotTracker.Register(shot, null, OnShotReached);
            shot.Mover.Move();
        }

        private void OnShotReached(Shot shot)
        {
            _currentCount++;

            _impactResolver.Resolve2(shot);

            ArmamentContext armamentContext = shot.Context;

            if (armamentContext.Target.PlatoonType == PlatoonType.Enemies)
            {
                StatusSetup statusSetup = _staticDataService.GetStatusSetup(StatusType.Stun);

                _statusResolver.Resolve(_statusFactory.Create(statusSetup, armamentContext.Target), armamentContext.Target);
            }
        }
    }
}