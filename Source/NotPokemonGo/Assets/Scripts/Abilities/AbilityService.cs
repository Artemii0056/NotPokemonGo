using System;
using System.Collections.Generic;
using System.Threading;
using Abilities.MV;
using AbilityNew;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Executors;
using AbilityNew.Scripts.Executors.Flow;
using AbilityNew.Scripts.Executors.Gameplay;
using AbilityNew.Scripts.Executors.Presentation;
using AbilityNew.Scripts.Steps.Flow;
using AbilityNew.Scripts.Steps.Gameplay;
using AbilityNew.Scripts.Steps.Presentation;
using Battlefields;
using Cysharp.Threading.Tasks;
using Effects;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using QteSystem;
using Services.AssetManagement;
using Spawners.Spawner;
using Statuses.Services;
using Units;
using Units.Movement;

namespace Abilities
{
    public sealed class AbilityService : IAbilityService, IDisposable
    {
        private const float PostDelaySeconds = 0.5f;

        private  AbilityRunner _abilityRunner;
        private readonly IResourceLoader _resourceLoader;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IStatusManager _statusManager;

        private Battlefield _battlefield;
        private Unit _lastUnit;
        private CancellationTokenSource _postFlowCts;
        private IQteService _qteService;
        private IArmamentSpawner _armamentSpawner;
        private IEffectResolver _effectResolver;

        public event Action Finished;

        public AbilityService(
            IResourceLoader resourceLoader,
            IBattleStateMachine battleStateMachine,
            IStatusManager statusManager, IQteService qteService, IArmamentSpawner armamentSpawner, IEffectResolver effectResolver)
        {
            _resourceLoader = resourceLoader;
            _battleStateMachine = battleStateMachine;
            _statusManager = statusManager;
            _qteService = qteService;
            _armamentSpawner = armamentSpawner;
            _effectResolver = effectResolver;
        }

        public void Dispose()
        {
            _postFlowCts?.Cancel();
            _postFlowCts?.Dispose();
            _postFlowCts = null;
        }

        public void SetBattlefield(Battlefield battlefield) =>
            _battlefield = battlefield;

        public async UniTaskVoid RunAbilityAsync(Unit source, Unit target, AbilityModel abilityModel) =>
            RunAbilityInternalAsync(source, target, abilityModel).Forget();

        public async UniTaskVoid RunAbilityInternalAsync(Unit source, Unit target, AbilityModel abilityModel)
        {
            _lastUnit = source;

            AbilitySO ability = ResolveAbility(source);

            var context = new AbilityExecutionContext(
                source,
                target,
                new  List<Unit>());

            var signalService = new SignalService();
            IUnitMover unitMover = new UnitMover();
            
            AnimationSignalRelay animationSignalRelay = new AnimationSignalRelay(signalService, source.AnimatorController);

            var executors = new Dictionary<Type, IAbilityStepExecutor>
            {
                { typeof(DamageStep), new DamageStepExecutor(_effectResolver) },
                { typeof(PlayAnimationStep), new PlayAnimationExecutor() },
                { typeof(WaitSignalStep), new WaitSignalExecutor(signalService) },
                { typeof(WaitingStep), new WaitingExecutor() },
                { typeof(StartQteStep), new StartQteExecutor(_qteService) },
                { typeof(SequenceStep), new SequenceExecutor() },

                { typeof(MoveStep), new MoveExecutor(unitMover) },
                { typeof(MoveBackStep), new MoveBackExecutor(unitMover) },
                { typeof(SpawnProjectileStep), new SpawnProjectileExecutor(_armamentSpawner) },
                { typeof(ArmamentMoverStep), new ArmamentMoverExecutor() },
            };
            
            StepExecutorRegistry stepExecutorRegistry = new StepExecutorRegistry(executors.Values);
            
            stepExecutorRegistry.AddExecutor(new RepeatExecutor(stepExecutorRegistry));
            stepExecutorRegistry.AddExecutor(new ParallelExecutor(stepExecutorRegistry));

            _abilityRunner = new AbilityRunner(stepExecutorRegistry);

            AbilityExecutionResult result = await _abilityRunner.RunAbility(ability, context);

            await ContinueAsync(result);
        }

        private AbilitySO ResolveAbility(Unit source)
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