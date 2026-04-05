using System;
using System.Collections.Generic;
using System.Threading;
using Abilities.MV;
using AbilityNew;
using AbilityNew.AbilityDefinition;
using AbilityNew.Diagnostics;
using AbilityNew.Scripts;
using AbilityNew.Scripts.Executors;
using AbilityNew.Scripts.Results;
using Battlefields;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using Services.AssetManagement;
using Services.StaticDataServices;
using Statuses.Services;
using Units;
using UnityEngine;

namespace Abilities
{
    public sealed class AbilityService : IAbilityService, IDisposable
    {
        private const float PostDelaySeconds = 0.5f;

        private readonly IResourceLoader _resourceLoader;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IStatusManager _statusManager;
        private readonly IStaticDataService _staticDataService;
        
        private readonly IAbilityStepExecutorRegistryFactory _abilityStepExecutorRegistryFactory;

        private Battlefield _battlefield;
        private Unit _lastUnit;
        private CancellationTokenSource _postFlowCts;

        public event Action Finished;

        public AbilityService(
            IResourceLoader resourceLoader,
            IBattleStateMachine battleStateMachine,
            IStatusManager statusManager,
            IStaticDataService staticDataService, 
            IAbilityStepExecutorRegistryFactory abilityStepExecutorRegistryFactory)
        {
            _resourceLoader = resourceLoader;
            _battleStateMachine = battleStateMachine;
            _statusManager = statusManager;
            _staticDataService = staticDataService;
            _abilityStepExecutorRegistryFactory = abilityStepExecutorRegistryFactory;
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

            AbilitySo ability = ResolveAbility(source, abilityModel);

            try
            {
                AbilityExecutionResult result = await ExecuteAbilityOnlyAsync(
                    source,
                    target,
                    ability);

                if (result.Completed == false)
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
            AbilitySo ability)
        {
            AbilityExecutionContext context = new(
                source,
                target,
                new List<Unit>());

            using CancellationTokenSource abilityCts = new();

            IAbilityTraceWriter writer = new FileAbilityTraceWriter();
            
            SignalService signalService = new();
            
            using AnimationSignalRelay animationSignalRelay =
                new(signalService, source.AnimatorController);

            AbilityPresentationConfig so;

            if (source.PlatoonType == PlatoonType.Heroes)
                so = _resourceLoader.Load<AbilityPresentationConfig>("BennetBaseAttackPresentation");
            else
                so = _resourceLoader.Load<AbilityPresentationConfig>("MageFireballAttackPresentation");

            StepExecutorRegistry stepExecutorRegistry = _abilityStepExecutorRegistryFactory.Create(signalService);
            _abilityStepExecutorRegistryFactory.PresentationService.Register(so);
            
            AbilityRunner abilityRunner = new(stepExecutorRegistry, signalService, writer);

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

                AbilitySo counterAbility = _staticDataService.GetCounterattackAbility(request.Reactor.UnitType);

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

        private AbilitySo ResolveAbility(Unit source, AbilityModel abilityModel)
        {
            return _staticDataService.GetAbility(source.UnitType, abilityModel.AbilityType);
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