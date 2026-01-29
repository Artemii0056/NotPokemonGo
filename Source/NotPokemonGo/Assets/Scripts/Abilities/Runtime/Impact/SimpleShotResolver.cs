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

            var phaseService = context?.AnimatorTrigger?.PhaseService;
            if (phaseService != null)
                phaseService.ArmamentRequested += OnArmamentRequested;
        }

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
        {
            _activePhase = phase;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            var phaseService = ctx?.AnimatorTrigger?.PhaseService;
            if (phaseService != null)
                phaseService.ArmamentRequested -= OnArmamentRequested;

            _shotTracker.CleanupAll();

            _context = null;
            _activePhase = null;
        }

        /// <summary>
        /// ✅ НИКАКОЙ зависимости от Finish-сигнала.
        /// ✅ НИКАКОЙ булки WaitForExternalCompletion.
        /// Логика простая:
        /// - если в фазе нет armament-действий → policy не блокирует
        /// - если armament-действия есть → ждём, пока все шоты завершатся (ActiveCount == 0)
        /// </summary>
        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;

            if (phase != _activePhase)
                return true;

            if (!PhaseHasArmament(phase))
                return true;

            return _shotTracker.ActiveCount == 0;
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

            var context = _context;
            if (context == null)
                return;

            // Если это шот текущей фазы — после его завершения можно попросить перепроверку.
            // (TryCompleteFinish rate-limited 1/кадр — спама не будет)
            if (shot.Phase != null && shot.Phase == _activePhase)
                context.AnimatorTrigger?.PhaseService?.RequestFinishCheck();
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
    }
}
