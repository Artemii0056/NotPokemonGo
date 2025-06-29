using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using Units;

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

            Unit unitSorce = _battleUnitContainer.Give();

            if (unitSorce != null)
            {
                _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(
                    new UnitActionPayload
                    (
                        unitSorce,
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