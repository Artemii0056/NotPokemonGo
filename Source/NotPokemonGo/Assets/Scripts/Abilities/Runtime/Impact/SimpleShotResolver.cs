using Abilities.Configs;
using Abilities.Runtime.Policies;
using Abilities.Signals;
using Armaments;
using Armaments.Spawner;
using Services.AbilityServices;

namespace Abilities.Runtime.Impact
{
    public sealed class SimpleShotsPolicy : AbilityPolicyBase
    {
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IShotImpactResolver _impactResolver;
        private readonly ShotTracker _shotTracker = new();

        private AbilityContext _context;

        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        public SimpleShotsPolicy(
            IArmamentSpawner armamentSpawner,
            IEffectsApplier effectsApplier,
            ImpactPolicy policyOverride = null)
        {
            _armamentSpawner = armamentSpawner;

            _impactResolver = new DefaultShotImpactResolver(
                effectsApplier,
                armamentSpawner,
                policyOverride,
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

            _shotTracker.CleanupAll();

            _context = null;
            _activePhase = null;
            _finishSeenForActivePhase = false;
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;

            if (phase == _activePhase)
            {
                if (!_finishSeenForActivePhase)
                    return false;

                if (!phase.WaitForExternalCompletion)
                    return true;

                return _shotTracker.ActiveCount == 0;
            }

            return true;
        }

        private void OnArmamentRequested(ArmamentRequest request)
        {
            foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(request))
            {
                var mover = _armamentSpawner.Create(ctx);

                var shot = new Shot(request.Phase, ctx, mover)
                {
                    RequiresQte = false,
                    QteResult = null
                };

                StartShot(shot);
            }
        }

        private void StartShot(Shot shot)
        {
            _shotTracker.Register(shot, onLaunched: null, onReached: OnShotReached);
            shot.Mover.Move();
        }

        private void OnShotReached(Shot shot)
        {
            _impactResolver.Resolve(shot);

            AbilityContext context = _context;
            
            if (context == null)
                return;

            AbilityPhase shotPhase = shot.Phase;

            if (shotPhase != null
                && shotPhase == _activePhase
                && shotPhase.WaitForExternalCompletion
                && _finishSeenForActivePhase)
            {
                if (_shotTracker.ActiveCount == 0)
                    context.Animator?.FlagSignal((int)PhaseSignal.Finish);
            }
        }
    }
}
