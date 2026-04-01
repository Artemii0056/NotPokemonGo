using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Abilities.MV;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Executors.Debugger;
using AbilityNew.Scripts.Executors.Flow;
using AbilityNew.Scripts.Executors.Gameplay;
using AbilityNew.Scripts.Executors.Presentation;
using AbilityNew.Scripts.Results;
using AbilityNew.Scripts.Steps.Gameplay;
using Battlefields;
using Cysharp.Threading.Tasks;
using Effects;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using QteSystem;
using Services.AssetManagement;
using Services.StaticDataServices;
using Spawners;
using Spawners.Spawner;
using Statuses.Services;
using Units;
using Units.Movement;
using UnityEngine;

namespace Abilities
{
    public sealed class AbilityService : IAbilityService, IDisposable
    {
        private const float PostDelaySeconds = 0.5f;

        private readonly IResourceLoader _resourceLoader;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IStatusManager _statusManager;
        private readonly IQteService _qteService;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IEffectResolver _effectResolver;
        private readonly ITargetSelector _targetSelector;
        private readonly IParticleSpawner _particleSpawner;
        private readonly IStaticDataService _staticDataService;

        private Battlefield _battlefield;
        private Unit _lastUnit;
        private CancellationTokenSource _postFlowCts;

        public event Action Finished;

        public AbilityService(
            IResourceLoader resourceLoader,
            IBattleStateMachine battleStateMachine,
            IStatusManager statusManager,
            IQteService qteService,
            IArmamentSpawner armamentSpawner,
            IEffectResolver effectResolver,
            ITargetSelector targetSelector,
            IParticleSpawner particleSpawner,
            IStaticDataService staticDataService)
        {
            _resourceLoader = resourceLoader;
            _battleStateMachine = battleStateMachine;
            _statusManager = statusManager;
            _qteService = qteService;
            _armamentSpawner = armamentSpawner;
            _effectResolver = effectResolver;
            _targetSelector = targetSelector;
            _particleSpawner = particleSpawner;
            _staticDataService = staticDataService;
        }

        public void Dispose()
        {
            _postFlowCts?.Cancel();
            _postFlowCts?.Dispose();
            _postFlowCts = null;
        }

        public void SetBattlefield(Battlefield battlefield) =>
            _battlefield = battlefield;

        public UniTask RunAbilityAsync(Unit source, Unit target, AbilityModel abilityModel) =>
            RunAbilityInternalAsync(source, target, abilityModel);

        private async UniTask RunAbilityInternalAsync(Unit source, Unit target, AbilityModel abilityModel)
        {
            _lastUnit = source;

            AbilitySO ability = ResolveAbility(source, abilityModel);

            try
            {
                AbilityExecutionResult result = await ExecuteAbilityOnlyAsync(
                    source,
                    target,
                    ability);

                if (!result.Completed)
                    return;

                await HandleCounterAttacksAsync(result.CounterAttackRequests);
                await ContinueAsync(result);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning($"Ability '{ability?.name}' was cancelled.");
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Ability '{ability?.name}' crashed. Source={source?.name}, Target={target?.name}\n{ex}");
                throw;
            }
        }

        private async UniTask<AbilityExecutionResult> ExecuteAbilityOnlyAsync(
            Unit source,
            Unit target,
            AbilitySO ability)
        {
            AbilityExecutionContext context = new(
                source,
                target,
                new List<Unit>());

            using CancellationTokenSource abilityCts = new();

            SignalService signalService = new();
            IUnitMover unitMover = new UnitMover();

            using AnimationSignalRelay animationSignalRelay =
                new(signalService, source.AnimatorController);

            List<IAbilityStepExecutor> executors = CreateExecutors(signalService, unitMover).ToList();
            StepExecutorRegistry registry = new(executors);

            registry.AddExecutor(new BranchExecutor(registry));
            registry.AddExecutor(new ParallelExecutor(registry));
            registry.AddExecutor(new RepeatExecutor(registry));
            registry.AddExecutor(new SequenceExecutor(registry));
            registry.AddExecutor(new ResolveQteExecutor(registry));

            AbilityRunner abilityRunner = new(registry);

            return await abilityRunner.RunAbility(ability, context, abilityCts.Token);
        }

        private async UniTask HandleCounterAttacksAsync(IReadOnlyList<CounterAttackRequest> requests)
        {
            if (requests == null || requests.Count == 0)
                return;

            foreach (CounterAttackRequest request in requests)
            {
                if (request == null)
                    continue;

                if (request.Reactor == null || request.Target == null)
                    continue;

                if (!request.Reactor.IsAlive || !request.Target.IsAlive)
                    continue;

                AbilitySO counterAbility = _staticDataService.GetCounterattackAbility(request.Reactor.UnitType);
                
                if (counterAbility == null)
                    continue;

                AbilityExecutionResult counterResult = await ExecuteAbilityOnlyAsync(
                    request.Reactor,
                    request.Target,
                    counterAbility);

                if (!counterResult.Completed)
                    break;
            }
        }

        private IEnumerable<IAbilityStepExecutor> CreateExecutors(
            SignalService signalService,
            IUnitMover unitMover)
        {
            return new IAbilityStepExecutor[]
            {
                new PlayAnimationExecutor(),
                new WaitSignalExecutor(signalService),
                new WaitingExecutor(),
                new StartQteExecutor(_qteService),
                new MoveExecutor(unitMover),
                new MoveBackExecutor(unitMover),
                new SpawnProjectileExecutor(_armamentSpawner),
                new ArmamentMoverExecutor(),
                new DamageStepExecutor(_effectResolver, _targetSelector),
                new SetBlackboardBoolExecutor(),
                new PrepareArmamentSpawnPointsExecutor(),
                new DebugExecutor(),
                new RequestCounterAttackExecutor()
            };
        }

        private AbilitySO ResolveAbility(Unit source, AbilityModel abilityModel)
        {
            AbilitySO so;

            if (source.PlatoonType == PlatoonType.Heroes)
                so = _resourceLoader.Load<AbilitySO>("BennetBaseAttack");
            else
                so = _resourceLoader.Load<AbilitySO>("MageFireballAttack");

            return so;
        }

        private async UniTask ContinueAsync(AbilityExecutionResult result)
        {
            _postFlowCts?.Cancel();
            _postFlowCts?.Dispose();
            _postFlowCts = new CancellationTokenSource();

            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(PostDelaySeconds),
                    cancellationToken: _postFlowCts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (_lastUnit != null && _lastUnit.PlatoonType == PlatoonType.Heroes)
                _statusManager.TickUnitTurn();

            _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(_battlefield);
            Finished?.Invoke();
        }
    }
}