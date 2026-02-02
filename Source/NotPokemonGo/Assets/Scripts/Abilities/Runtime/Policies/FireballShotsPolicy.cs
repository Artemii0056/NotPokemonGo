using Abilities.Configs;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments;
using Armaments.Movers;
using Effects;
using QteSystem;
using Services.AbilityServices;
using Spawners.Spawner;

namespace Abilities.Runtime.Policies
{
    public sealed class FireballShotsPolicy : AbilityPolicyBase
    {
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IShotImpactResolver _impactResolver;
        private readonly ShotTracker _shotTracker = new();
        private readonly QteBinder _qteBinder;

        private AbilityContext _context;

        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        public FireballShotsPolicy(
            IQteService qteService,
            IArmamentSpawner armamentSpawner,
            IEffectsApplier effectsApplier)
        {
            _armamentSpawner = armamentSpawner;
            _qteBinder = new QteBinder(qteService);

            var policy = new ImpactPolicy
            {
                NoQteAction = ImpactAction.ApplyEffectsAndDestroy,
                OnFail = ImpactAction.ApplyEffectsAndDestroy,
                OnNormal = ImpactAction.DestroyOnly,
                OnPerfect = ImpactAction.ReflectToSourceAndDestroy
            };

            _impactResolver = new DefaultShotImpactResolver(
                effectsApplier,
                armamentSpawner,
                policy,
                startShot: StartShot);
        }

        public override void OnAbilityStart(AbilityContext context)
        {
            _context = context;

            AbilityPhaseService phaseService = context?.AnimatorTrigger?.PhaseService;

            if (phaseService != null)
                phaseService.ArmamentRequested += OnArmamentRequested;
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
        {
            _activePhase = phase;
            _finishSeenForActivePhase = false;
        }

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            if (signal == PhaseSignal.Finish && ctx != null && ctx.CurrentPhase == _activePhase)
                _finishSeenForActivePhase = true;
        }

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
                var a = actions[i];
                
                if (a != null && a.HasArmament)
                    return true;
            }

            return false;
        }

        private void OnArmamentRequested(ArmamentRequest request)
        {
            foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(request))
            {
                IArmamentMover mover = _armamentSpawner.Create(ctx);

                Shot shot = new Shot(request.Phase, ctx, mover)
                {
                    RequiresQte = request.Phase.QteType != QteType.Unknown
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