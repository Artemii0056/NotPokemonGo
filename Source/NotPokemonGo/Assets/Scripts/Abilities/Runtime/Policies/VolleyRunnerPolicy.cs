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
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStatusFactory _statusFactory;
        private readonly IQteService _qteService;
        private readonly IStaticDataService _staticDataService;
        private readonly IStatusResolver _statusResolver;
        private readonly ShotCoordinator _shotCoordinator = new();
        private readonly BaseArmamentImpactResolver _baseArmamentImpactResolver;
        private IQteSession _qteSession;

        private AbilityContext _context;

        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        private Coroutine _coroutine;

        private int _currentCount;

        private int _shotCount;

        private bool _haveCounterAttack;

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

            _baseArmamentImpactResolver = new BaseArmamentImpactResolver(effectsApplier, armamentSpawner, StartShot);
            _haveCounterAttack = false;
        }

        public override bool CanUseAbility(AbilityContext ctx) =>
            ctx.Movers.Count > 0;

        public override void OnAbilityStart(AbilityContext context)
        {
            if (context.Movers.Count == 0)
                return;

            _context = context;
            
            _shotCount = context.Movers.Count;
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

                _context.Movers.Remove(mover);
                StartShot(mover);

                yield return new WaitForSeconds(0.3f);
            }
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            if (signal == PhaseSignal.Finish && ctx != null && ctx.CurrentPhase == _activePhase)
                _finishSeenForActivePhase = true;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            _shotCoordinator.CleanupAll();

            _context = null;
            _activePhase = null;
            _finishSeenForActivePhase = false;
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase) //Тут должно произойти окончание.
                                                                                    //Он сам должен сообщить, когда частично или полностью закончил что нужно
        {
            if (phase == null)
                return true;

            if (!_finishSeenForActivePhase)
                return false;

            bool canFinishPhase = _shotCoordinator.ActiveCount == 0 &&
                                  _currentCount >= _shotCount
                                  && _haveCounterAttack;

            if (canFinishPhase) 
                _qteSession.Dispose();
            
            Debug.Log(canFinishPhase);
            Debug.Log($"{_shotCoordinator.ActiveCount} Count {canFinishPhase} ");
            
            return canFinishPhase;
        }

        private void StartShot(IArmamentMover mover)
        {
            _shotCoordinator.Register(mover, OnShotReached);
            mover.Move();
        }
        
        private void OnShotReached(IArmamentMover mover) 
        {
            _currentCount++;

            _baseArmamentImpactResolver.Resolve(mover);
            
            ArmamentContext armamentContext = mover.Armament.Context;

            if (armamentContext.Target.PlatoonType == PlatoonType.Enemies)
            {
                _haveCounterAttack = false;
                
                StatusSetup statusSetup = _staticDataService.GetStatusSetup(StatusType.Stun);

                _statusResolver.Resolve(_statusFactory.Create(statusSetup, armamentContext.Target), armamentContext.Target);
            }
        }
    }
}