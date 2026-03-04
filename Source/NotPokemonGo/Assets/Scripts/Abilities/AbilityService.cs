using System;
using System.Collections.Generic;
using System.Threading;
using Abilities.Bennet;
using Abilities.Flow;
using Abilities.MV;
using Battlefields;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Platoons;
using Statuses.Services;
using UnityEngine;
using Unit = Units.Unit;

namespace Abilities
{
    public sealed class AbilityService : IAbilityService, IDisposable
    {
        private const float PostDelaySeconds = 0.5f;

        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IStatusManager _statusManager;
        private readonly IAbilityHandlerFactory _abilityHandlerFactory;

        private Battlefield _battlefield;
        private readonly HashSet<IAbilityHandler> _activeAbilityHandlers = new();

        private Unit _lastUnit;

        private CancellationTokenSource _postFlowCts;

        public event Action Finished;

        public AbilityService(
            IBattleStateMachine battleStateMachine,
            IStatusManager statusManager,
            IAbilityHandlerFactory abilityHandlerFactory)
        {
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

        public void SetBattlefield(Battlefield battlefield) => _battlefield = battlefield;

        public void Handle(Unit source, Unit target, AbilityModel abilityModel)
        {
            if (abilityModel == null)
            {
                Debug.LogError("[AbilityService] abilityModel is null");
                return;
            }

            IAbilityHandler handler = _abilityHandlerFactory.Create(source, target, abilityModel);
            if (handler == null)
            {
                Debug.LogError($"[AbilityService] HandlerFactory returned null for {abilityModel.AbilityType}");
                return;
            }

            handler.Finished += Continue;
            handler.Play(source, target);
            _activeAbilityHandlers.Add(handler);

            _lastUnit = source;

            // TODO: убрать из Unit, сделать отдельный слой.
            source.RememberAbility(handler);
        }

        public void HandleCounterAttack(Unit source, Unit target, AbilityModel abilityModel)
        {
            if (abilityModel == null)
            {
                Debug.LogError("[AbilityService] Counter abilityModel is null");
                return;
            }

            IAbilityHandler handler = _abilityHandlerFactory.Create(source, target, abilityModel);
            if (handler == null)
            {
                Debug.LogError($"[AbilityService] Counter handler is null for {abilityModel?.AbilityType}");
                return;
            }

            _activeAbilityHandlers.Add(handler);
            handler.Play(source, target);
            handler.Finished += Continue;
        }

        private void Continue(IAbilityHandler handler) => ContinueAsync(handler).Forget();

        private async UniTaskVoid ContinueAsync(IAbilityHandler handler)
        {
            if (handler != null)
            {
                _activeAbilityHandlers.Remove(handler);
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
