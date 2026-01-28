using System;
using Abilities.Configs;
using Abilities.Runtime.Impact;
using Abilities.Signals;
using Armaments;
using Armaments.Spawner;
using QTESystem;
using Services.AbilityServices;

namespace Abilities.Runtime.Policies
{
    /// <summary>
    /// Универсальная политика для армаментных фаз.
    /// - Подписывается на ArmamentRequested
    /// - Спавнит выстрелы, вешает QTE при необходимости
    /// - Управляет завершением фазы (WaitForExternalCompletion + все шоты/QTE отработали)
    /// </summary>
    public sealed class ArmamentShotsPolicy : AbilityPolicyBase
    {
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IShotImpactResolver _impactResolver;
        private readonly ShotTracker _shotTracker = new();
        private readonly QteBinder _qteBinder;
        private readonly bool _useQte;

        private AbilityContext _context;
        private AbilityPhase _activePhase;
        private bool _finishSeenForActivePhase;

        public ArmamentShotsPolicy(
            IArmamentSpawner armamentSpawner,
            IEffectsApplier effectsApplier,
            IQteService qteService,
            ImpactPolicy impactPolicy,
            bool useQte)
        {
            _armamentSpawner = armamentSpawner ?? throw new ArgumentNullException(nameof(armamentSpawner));
            _useQte = useQte;
            _qteBinder = new QteBinder(qteService);

            _impactResolver = new DefaultShotImpactResolver(
                effectsApplier,
                armamentSpawner,
                impactPolicy,
                StartShot);
        }

        public override void OnAbilityStart(AbilityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            AbilityPhaseService phaseService = _context.AnimatorTrigger?.PhaseService;

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

            if (phase == _activePhase)
            {
                if (!_finishSeenForActivePhase)
                    return false;

                if (!phase.WaitForExternalCompletion)
                    return true;

                bool noShots = _shotTracker.ActiveCount == 0;
                bool noQte = !_useQte && _qteBinder.ActiveCount == 0;

                return noShots && noQte;
            }

            return true;
        }

        private void OnArmamentRequested(ArmamentRequest request)
        {
            foreach (var ctx in ArmamentRequestMapper.EnumerateContexts(request))
            {
                IArmamentMover mover = _armamentSpawner.Create(ctx);

                bool phaseHasQte = _useQte && request.Phase != null &&
                                   request.Phase.QteType != QTESystem.QteType.Unknown;

                Shot shot = new Shot(request.Phase, ctx, mover)
                {
                    RequiresQte = phaseHasQte
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
            if (!shot.RequiresQte)
                return;

            _qteBinder.BindOnLaunch(
                shot,
                qteTarget: shot.Context.Target,
                timeoutPolicy: QteTimeoutPolicy.TreatAsFail);
        }

        private void OnShotReached(Shot shot)
        {
            if (shot.RequiresQte)
                _qteBinder.UnbindOnReach(shot);

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
                bool noShots = _shotTracker.ActiveCount == 0;
                bool noQte = !_useQte && _qteBinder.ActiveCount == 0;

                if (noShots && noQte)
                    context.Animator?.FlagSignal((int)PhaseSignal.Finish);
            }
        }
    }
}