using Battlefields;
using Infrastructure.StateMachines.States.Interfaces;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class UpdateBattleTickState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;

        public UpdateBattleTickState(IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }
        
        public void Enter(Battlefield battlefield)
        {
            battlefield.Tick(Time.deltaTime);
            _battleStateMachine.Enter<SelectReadyUnitState, Battlefield>(battlefield);
        }

        public void Exit()
        {
            
        }
    }
}