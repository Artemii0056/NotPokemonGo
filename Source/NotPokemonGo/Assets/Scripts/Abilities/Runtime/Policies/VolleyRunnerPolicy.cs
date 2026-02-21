using System.Collections;
using Abilities.Configs;
using Abilities.Runtime;
using Abilities.Runtime.Impact;
using Abilities.Runtime.Policies;
using Abilities.Signals;
using Armaments.Movers;
using Effects;
using QteSystem;
using ReactionSystems;
using Services;
using UnityEngine;

public class VolleyRunnerPolicy : AbilityPolicyBase
{
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IQteService _qteService;
    private readonly IReactionService _reactionService;
    private readonly BaseArmamentImpactResolver _impactResolver;
    private readonly ShotCoordinator _shotCoordinator = new();

    private AbilityContext _context;
    private AbilityPhase _activePhase;

    private Coroutine _coroutine;
    private IQteSession _qteSession;

    private bool _finishSignalReceived;

    public VolleyRunnerPolicy(
        IQteService qteService,
        ICoroutineRunner coroutineRunner,
        IEffectsApplier effectsApplier,
        IReactionService reactionService)
    {
        _qteService = qteService;
        _coroutineRunner = coroutineRunner;
        _reactionService = reactionService;

        _impactResolver = new BaseArmamentImpactResolver(effectsApplier);
    }

    public override bool CanUseAbility(AbilityContext ctx) =>
        ctx.Movers.Count > 0;

    public override void OnAbilityStart(AbilityContext context)
    {
        _context = context;
    }

    public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
    {
        _context = ctx;
        _activePhase = phase;
        _finishSignalReceived = false;

        _qteSession = _qteService.StartSession(
            ctx.CurrentPhase.QteType,
            ctx.Target,
            100);

        _coroutine = _coroutineRunner.StartCoroutine(LaunchShots());
    }

    private IEnumerator LaunchShots()
    {
        while (_context.Movers.Count > 0)
        {
            var mover = _context.Movers[0];
            _context.Movers.RemoveAt(0);

            RegisterShot(mover);

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void RegisterShot(IArmamentMover mover)
    {
        _shotCoordinator.Register(mover, OnShotReached);
        mover.Move();
    }

    private void OnShotReached(IArmamentMover mover)
    {
        var reactionContext = new ReactionContext
        {
            Source = mover.Armament.Source,
            Target = mover.Armament.Target,
            Armament = mover.Armament,
            SpawnShot = RegisterShot
        };

        _reactionService.TryReact(reactionContext);

        if (!reactionContext.WasReflected)
        {
            _impactResolver.Resolve(mover);
        }
        else
        {
            Object.Destroy(mover.Armament.gameObject);
        }
    }

    public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
    {
        if (ctx.CurrentPhase != _activePhase)
            return;

        if (signal == PhaseSignal.Finish)
            _finishSignalReceived = true;
    }

    public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
    {
        if (phase != _activePhase)
            return false;

        if (!_finishSignalReceived)
            return false;

        if (_shotCoordinator.ActiveCount > 0)
            return false;

        _qteSession?.Dispose();
        return true;
    }

    public override void OnAbilityStop(AbilityContext ctx)
    {
        _shotCoordinator.CleanupAll();

        if (_coroutine != null)
            _coroutineRunner.StopCoroutine(_coroutine);

        _qteSession?.Dispose();

        _context = null;
        _activePhase = null;
        _finishSignalReceived = false;
    }
}
