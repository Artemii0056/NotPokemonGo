using Infrastructure.StateMachines.BattleStateMachine;
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
        
        public void Enter(LevelRuntimeDataPayload payload)
        {
            _battleInfoUI = payload.BattleInfoUI;
            _battleInfoUI.gameObject.SetActive(true);
            
            _battleInfoUI.SetValue(1);
            
            _battleStateMachine.Enter<WaveProgressionState>();
        }

        public void Exit()
        {
        }
    }
}