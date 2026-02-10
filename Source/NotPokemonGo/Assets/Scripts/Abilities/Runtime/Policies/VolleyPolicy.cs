using Abilities.Configs;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments.Movers;
using QteSystem;
using Services.AbilityServices;
using Spawners.Spawner;

namespace Abilities.Runtime.Policies
{
    public class
        VolleyPolicy : AbilityPolicyBase //Логика класса. При сигнале создается несколько фаерболов над головой у держаетля.
    {
        private readonly IAbilityService _abilityService;
        private readonly IShotImpactResolver _shotImpactResolver;
        private readonly IQteService _qteService;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly QteBinder _qteBinder;
        private readonly ShotTracker _shotTracker;

        private AbilityContext _context;

        private ArmamentRequest _armamentRequest;
        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        public VolleyPolicy(
            IAbilityService abilityService,
            IShotImpactResolver shotImpactResolver,
            ShotTracker shotTracker,
            IQteService qteService,
            IArmamentSpawner armamentSpawner)
        {
            _abilityService = abilityService;
            _shotImpactResolver = shotImpactResolver;
            _qteBinder = new QteBinder(qteService);
            _shotTracker = shotTracker;
            _qteService = qteService;
            _armamentSpawner = armamentSpawner;
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
        {
            _activePhase = phase;
            _finishSeenForActivePhase = false;
        }

        public override void OnAbilityStart(AbilityContext context)
        {
            _context = context;

            AbilityPhaseService phaseService = context?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested += OnArmamentRequested;
        }

        private void OnArmamentRequested(ArmamentRequest request)
        {
            _armamentRequest = request;

            for (int i = 0; i < 3; i++)
            {
                foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(request))
                {
                    IArmamentMover mover = _armamentSpawner.Create(ctx);

                    _context.Movers.Add(mover);
                }
            }
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal) //Тут должна запуститься корутина? 
        {
            // Debug.Log($"[FireballShotsPolicy] signal={signal} phaseMatch={ctx.CurrentPhase == _activePhase}");

            if (signal == PhaseSignal.Finish && ctx != null && ctx.CurrentPhase == _activePhase)
                _finishSeenForActivePhase = true;
        }

        // private void OnArmamentRequested(ArmamentRequest request) => 
        //     _armamentRequest = request;

        public override void OnAbilityStop(AbilityContext ctx)
        {
            var phaseService = ctx?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested -= OnArmamentRequested;

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
            _qteBinder.UnbindOnReach(shot);
            _shotImpactResolver.Resolve(shot);

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