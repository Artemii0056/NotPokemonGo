using System;
using System.Collections.Generic;
using System.Threading;
using Abilities.Bennet;
using Abilities.Flow;
using Abilities.MV;
using Abilities.Runtime;
using AbsolutelyNewPerfectAbilitySystem;
using AbsolutelyNewPerfectAbilitySystem.Scripts;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps;
using AbsolutelyNewPerfectAbilitySystem.Scripts.Executors;
using Battlefields;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using Services.AssetManagement;
using Spawners.Spawner;
using Statuses.Services;
using Units.Movement;
using ParallelStep = AbsolutelyNewPerfectAbilitySystem.Scripts.Configs.CompositeSteps.ParallelStep;
using Unit = Units.Unit;

namespace Abilities
{
    public sealed class AbilityService : IAbilityService, IDisposable
    {
        private readonly IResourceLoader _resourceLoader;

        private const float PostDelaySeconds = 0.5f;

        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IStatusManager _statusManager;
        private readonly IAbilityHandlerFactory _abilityHandlerFactory;
        private readonly IArmamentSpawner _armamentSpawner;

        private Battlefield _battlefield;

        private Unit _lastUnit;

        private CancellationTokenSource _postFlowCts;

        public event Action Finished;

        public AbilityService(
            IBattleStateMachine battleStateMachine,
            IStatusManager statusManager,
            IAbilityHandlerFactory abilityHandlerFactory,
            IResourceLoader resourceLoader, IArmamentSpawner armamentSpawner)
        {
            _resourceLoader = resourceLoader;
            _armamentSpawner = armamentSpawner;
            _battleStateMachine = battleStateMachine;
            _statusManager = statusManager;
            _abilityHandlerFactory = abilityHandlerFactory;
        }

        public void Dispose()
        {
            _postFlowCts?.Cancel();
            _postFlowCts?.Dispose();
            _postFlowCts = null;
        }

        public void SetBattlefield(Battlefield battlefield) => 
            _battlefield = battlefield;

        public async UniTaskVoid RunAbilityAsync(Unit source, Unit target, AbilityModel abilityModel)
        {
            AbilitySO so;
            
            SignalService signalService = new SignalService();
            IUnitMover unitMover = new UnitMover();

            AnimationSignalRelay animationSignalRelay = new AnimationSignalRelay(signalService, source.AnimatorController);
            
            var registry = new StepExecutorRegistry(
                new Dictionary<Type, IAbilityStepExecutor>
                {
                    { typeof(DamageStep), new DamageExecutor() },
                    { typeof(MoveStep), new MoveExecutor(unitMover) },
                    { typeof(PlayAnimationStep), new PlayAnimationExecutor() },
                    { typeof(WaitSignalStep), new WaitSignalExecutor(signalService) },
                    { typeof(MoveBackStep), new MoveBackExecutor(unitMover) },
                    { typeof(SpawnProjectileStep), new SpawnProjectileExecutor(_armamentSpawner) },
                    { typeof(ArmamentMoverStep), new ArmamentMoverExecutor() },
                    { typeof(WaitingStep), new WaitingExecutor() },
                }
            );
            
            registry.AddExecutor(typeof(RepeatStep), new RepeatExecutor(registry));
            registry.AddExecutor(typeof(ParallelStep), new ParallelExecutor(registry));
            
            AbilityRunner runner = new AbilityRunner(registry);

            AbilityContext abilityContext = new AbilityContext();
            abilityContext.Target = target;
            abilityContext.Source = source;
            abilityContext.StartPosition = source.StartPosition;

            if (source.PlatoonType == PlatoonType.Heroes)
                so = _resourceLoader.Load<AbilitySO>("BennetBaseAttack");
            else
                so = _resourceLoader.Load<AbilitySO>("MageFireballAttack");

            await runner.RunAbility(so, abilityContext);
            
            Continue(null);
        }

        private void Continue(IAbilityHandler handler) =>
            ContinueAsync(handler).Forget();

        private async UniTaskVoid ContinueAsync(IAbilityHandler handler)
        {
            if (handler != null)
            {
                handler.Finished -= Continue;
            }

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