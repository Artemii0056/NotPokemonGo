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
        private readonly IStatusManager _statusManager;

        public SelectReadyUnitState(IBattleStateMachine battleStateMachine, IBattleUnitContainer battleUnitContainer, IStatusManager  statusManager)
        {
            _battleStateMachine = battleStateMachine;
            _battleUnitContainer = battleUnitContainer;
            _statusManager = statusManager;
        }
        
        public void Enter(Battlefield batlfield)
        {
            foreach (Unit unit in batlfield.Units) 
                _battleUnitContainer.Add(unit);

            Unit unitSource = _battleUnitContainer.Give();

            if (unitSource != null)
            {
                _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(
                    new UnitActionPayload
                    (
                        unitSource,
                        batlfield)
                );
                
                batlfield.Units.Clear();
            }
            else
            {
                _battleStateMachine.Enter<UpdateBattleTickState, Battlefield>(batlfield);
            }
        }

        public void Exit()
        {
        }
    }
}