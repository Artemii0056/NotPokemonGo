using Abilities.Configs;
using Abilities.General;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments;
using Armaments.Spawner;
using QTESystem;
using Services.AbilityServices;

namespace Abilities.Runtime.Policies
{
    public sealed class FireballShotsPolicy : AbilityPolicyBase
    {
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly ShotTracker _shotTracker = new();
        private readonly QteBinder _qteBinder;
        private readonly IShotImpactResolver _impactResolver;

        private AbilityContext _ctx;

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

        public override void OnAbilityStart(AbilityContext ctx)
        {
            _ctx = ctx;

            var phaseService = ctx?.AnimatorTrigger?.PhaseService;
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

            _ctx = null;
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

                return _shotTracker.ActiveCount == 0 && _qteBinder.ActiveCount == 0;
            }

            return true;
        }

        private void OnArmamentRequested(ArmamentRequest req)
        {
            foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(req))
            {
                var mover = _armamentSpawner.Create(ctx);

                var shot = new Shot(req.Phase, ctx, mover)
                {
                    RequiresQte = req.Phase.QteType != QTESystem.QteType.Unknown
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

            var ctx = _ctx;
            
            if (ctx == null)
                return;

            var shotPhase = shot.Phase;

            if (shotPhase != null
                && shotPhase == _activePhase
                && shotPhase.WaitForExternalCompletion
                && _finishSeenForActivePhase)
            {
                if (_shotTracker.ActiveCount == 0 && _qteBinder.ActiveCount == 0)
                {
                    ctx.Animator?.FlagSignal((int)PhaseSignal.Finish);
                }
            }
        }
    }
}
