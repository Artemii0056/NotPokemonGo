using Infrastructure.StateMachines.BattleStateMachine;
using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class GlobalBattleState : IPayloadedState<LevelRuntimeDataPayload>
    {
        private readonly IBattleStateMachine _battleStateMachine;
        
        private BattleInfoUI _battleInfoUI; //TODO Часть из WaveProgress перенести сюда.
        
        public GlobalBattleState(IBattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }
        
        public void Enter(LevelRuntimeDataPayload battlefield)
        {
            _battleInfoUI = battlefield.BattleInfoUI;
            _battleInfoUI.gameObject.SetActive(true);
            
            _battleStateMachine.Enter<WaveProgressionState>();
        }

        public void Exit()
        {
        }
    }
}