using Infrastructure.StateMachines.States.Interfaces;

namespace Infrastructure.StateMachines.BattleStateMachine.States
{
    public class BattleUpgradeSelectionState  : IPayloadedState<Battlefield>
    {
        private readonly IBattleStateMachine _battleStateMachine;

        public BattleUpgradeSelectionState (IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }
        
        public void Enter(Battlefield unitActionPayload)
        {
        }

        public void Exit()
        {
            
        }
    }
}