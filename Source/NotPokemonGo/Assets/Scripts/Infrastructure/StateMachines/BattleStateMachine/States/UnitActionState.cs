using System;
using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using InputServices;
using Services.InputServices;
using UnityEngine;
using VContainer;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UnitActionState : IPayloadedState<UnitActionPayload>
    {
        private readonly IObjectResolver _objectResolver;

        private UnitActionStrategy _unitActionStrategy;
        private IInputReader _inputReader;
        private IBattleStateMachine _battleStateMachine;
        private UnitActionPayload _payload;

        public UnitActionState(IObjectResolver objectResolver, IInputReader inputReader, IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            _inputReader = inputReader;
            _objectResolver = objectResolver;
        }

        public void Enter(UnitActionPayload unitActionPayload)
        {
            _payload = unitActionPayload;
            _inputReader.SpacePressed += SetFinishBattleState;

            _inputReader.EButtonPressed += SetQTEState;

            switch (unitActionPayload.UnitSorce.PlatoonType)
            {
                case PlatoonType.Friends:
                    _unitActionStrategy = new FriendUnitActionStrategy(unitActionPayload.Battlefield, unitActionPayload.UnitSorce);
                    break;

                case PlatoonType.Enemies:
                    _unitActionStrategy = new EnemyUnitActionStrategy(unitActionPayload.Battlefield, unitActionPayload.UnitSorce);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _objectResolver.Inject(_unitActionStrategy);

            _unitActionStrategy.Enable();        
        }

        private void SetQTEState()
        {
            _battleStateMachine.Enter<QTEBattleState, Battlefield>(_payload.Battlefield);
        }

        public void Exit()
        {
            _inputReader.EButtonPressed -= SetQTEState;

            _inputReader.SpacePressed -= SetFinishBattleState;
            _unitActionStrategy.Disable();
        }

        private void SetFinishBattleState()
        {
            _battleStateMachine.Enter<FinishBattleState, UnitActionPayload>(_payload);
        }
    }
}