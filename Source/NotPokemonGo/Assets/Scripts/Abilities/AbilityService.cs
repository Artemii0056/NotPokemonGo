using System;
using System.Collections.Generic;
using System.Threading;
using Abilities.MV;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Executors.Flow;
using AbilityNew.Scripts.Executors.Gameplay;
using AbilityNew.Scripts.Executors.Presentation;
using Battlefields;
using Cysharp.Threading.Tasks;
using Effects;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using QteSystem;
using Services.AssetManagement;
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

        private AbilityRunner _abilityRunner;
        private Battlefield _battlefield;
        private Unit _lastUnit;
        private CancellationTokenSource _postFlowCts;

        private readonly IResourceLoader _resourceLoader;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IStatusManager _statusManager;

        private readonly IQteService _qteService;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IEffectResolver _effectResolver;
        private readonly ITargetSelector _targetSelector;
        private readonly IParticleSpawner _particleSpawner;

        public event Action Finished;

        public AbilityService(
            IResourceLoader resourceLoader,
            IBattleStateMachine battleStateMachine,
            IStatusManager statusManager,
            IQteService qteService,
            IArmamentSpawner armamentSpawner,
            IEffectResolver effectResolver,
            ITargetSelector targetSelector,
            IParticleSpawner particleSpawner)
        {
            _resourceLoader = resourceLoader;
            _battleStateMachine = battleStateMachine;
            _statusManager = statusManager;
            _qteService = qteService;
            _armamentSpawner = armamentSpawner;
            _effectResolver = effectResolver;
            _targetSelector = targetSelector;
            _particleSpawner = particleSpawner;
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

            AbilitySO ability = ResolveAbility(source);

            var context = new AbilityExecutionContext(
                source,
                target,
                new List<Unit>());

            using var abilityCts = new CancellationTokenSource();

            SignalService signalService = new SignalService();
            IUnitMover unitMover = new UnitMover();
            using var animationSignalRelay = new AnimationSignalRelay(signalService, source.AnimatorController);

            var executors = CreateExecutors(signalService, unitMover);

            StepExecutorRegistry stepExecutorRegistry = new StepExecutorRegistry(executors);

            BranchExecutor branchExecutor = new BranchExecutor(stepExecutorRegistry);
            stepExecutorRegistry.AddExecutor(branchExecutor);

            _abilityRunner = new AbilityRunner(stepExecutorRegistry);

            try
            {
                AbilityExecutionResult result = await _abilityRunner.RunAbility(
                    ability,
                    context,
                    abilityCts.Token);

                if (result.Completed)
                {
                    await ContinueAsync(result);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning($"Ability '{ability.name}' was cancelled.");
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"Ability '{ability?.name}' crashed. Source={source?.name}, Target={target?.name}\n{ex}");
                throw;
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
            };
        }

        private AbilitySO ResolveAbility(Unit source)
        {
            AbilitySO so;

            if (source.PlatoonType == PlatoonType.Heroes)
                so = _resourceLoader.Load<AbilitySO>("HealthPercentTestAbility");
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
                await UniTask.Delay(TimeSpan.FromSeconds(PostDelaySeconds), cancellationToken: _postFlowCts.Token);
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