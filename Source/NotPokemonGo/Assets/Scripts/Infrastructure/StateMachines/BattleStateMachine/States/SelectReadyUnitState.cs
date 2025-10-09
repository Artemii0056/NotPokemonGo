using Battlefields;
using Infrastructure.StateMachines.BattleStateMachine.Payloads;
using Infrastructure.StateMachines.States.Interfaces;
using Services.BattleSessionService;
using Units;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class SelectReadyUnitState : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;
        
        private readonly IUnitReadyService _unitReadyService;

        public SelectReadyUnitState(
            IBattleStateMachine battleStateMachine, 
            IUnitReadyService unitReadyService)
        {
            _battleStateMachine = battleStateMachine;
            _unitReadyService = unitReadyService;
        }
        
        public void Enter(Battlefield battlefield) 
        {
            if (_unitReadyService.HasUnits)
            {
                Unit unitSource = _unitReadyService.GiveReadyUnit();
                
                _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(
                    new UnitActionPayload
                    (
                        unitSource,
                        battlefield)
                );
                
              //  battlefield.Units.Clear();
            }
            else
            {
                _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(battlefield);
            }
            
            // Debug.Log(battlefield.GetHashCode() + "SelectReadyUnitState");
            //
            // foreach (Unit unit in battlefield.Units) 
            //     _battleUnitContainer.Add(unit);
            //
            // Unit unitSource = _battleUnitContainer.Give();
            //
            // if (unitSource != null)
            // {
            //     _battleStateMachine.Enter<UnitActionState, UnitActionPayload>(
            //         new UnitActionPayload
            //         (
            //             unitSource,
            //             battlefield)
            //     );
            //     
            //     battlefield.Units.Clear();
            // }
            // else
            // {
            //     _battleStateMachine.Enter<CheckBattleEndState, Battlefield>(battlefield);
            // }
        }

        public void Exit()
        {
        }
    }
}