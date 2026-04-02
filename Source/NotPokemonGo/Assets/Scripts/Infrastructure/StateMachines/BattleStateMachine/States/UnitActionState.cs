using System;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using Platoons;
using Services.InputServices;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UnitActionState : IPayloadedState<UnitActionPayload>
    {
        private readonly IObjectResolver _objectResolver;
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IInputReader _inputReader;

        private UnitActionStrategy _unitActionStrategy;
        private UnitActionPayload _payload;

        public UnitActionState(
            IObjectResolver objectResolver, 
            IInputReader inputReader,
            IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            _inputReader = inputReader;
            _objectResolver = objectResolver;
        }

        public void Enter(UnitActionPayload battlefield)
        {
            _payload = battlefield;
            _inputReader.SpacePressed += SetFinishBattleState;

            switch (battlefield.UnitSorce.PlatoonType)
            {
                case PlatoonType.Heroes:
                    _unitActionStrategy = new FriendUnitActionStrategy(battlefield.Battlefield, battlefield.UnitSorce);
                    break;

                case PlatoonType.Enemies:
                    _unitActionStrategy = new EnemyUnitActionStrategy(battlefield.Battlefield, battlefield.UnitSorce);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _objectResolver.Inject(_unitActionStrategy);

            _unitActionStrategy.Enable();
        }

        public void Exit()
        {
            _inputReader.SpacePressed -= SetFinishBattleState;
            _unitActionStrategy.Disable();
        }

        private void SetFinishBattleState()
        {
            _battleStateMachine.Enter<FinishBattleState, UnitActionPayload>(_payload);
        }
    }
}