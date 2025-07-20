using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using Statuses.Services;
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
        
        public void Enter(Battlefield unitActionPayload)
        {
            foreach (Unit unit in unitActionPayload.Units) 
                _battleUnitContainer.Add(unit);

            Unit unitSource = _battleUnitContainer.Give();

            if (unitSource != null)
            {
                _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(
                    new UnitActionPayload
                    (
                        unitSource,
                        unitActionPayload)
                );
                
                unitActionPayload.Units.Clear();
            }
            else
            {
                _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(unitActionPayload);
            }
        }

        public void Exit()
        {
        }
    }
}