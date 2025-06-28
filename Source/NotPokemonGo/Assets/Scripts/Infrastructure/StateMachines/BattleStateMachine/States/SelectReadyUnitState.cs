using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using Services.BattleUnitContainers;
using Units;
using UnityEngine;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class SelectReadyUnitState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;
        private readonly IBattleUnitContainer _battleUnitContainer;

        public SelectReadyUnitState(IBattleStateMachine battleStateMachine, IBattleUnitContainer battleUnitContainer)
        {
            _battleStateMachine = battleStateMachine;
            _battleUnitContainer = battleUnitContainer;
        }
        
        public void Enter(Battlefield batlfield)
        {
            foreach (Unit unit in batlfield.Units) 
                _battleUnitContainer.Add(unit);
            
            _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(
                new UnitActionPayload
                (
                    _battleUnitContainer.Give(),
                    batlfield)
                );
        }

        public void Exit()
        {
        }
    }
}