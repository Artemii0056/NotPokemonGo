using Infrastructure.StateMachines.BattleStateMachine.States;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Infrastructure.StateMachines.States.Interfaces;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine
{
    public class LoosePanelState : IPayloadedState<LoosePanel>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private LoosePanel _loosePanel;
        
        public LoosePanelState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter(LoosePanel battlefield)
        {
            _loosePanel = battlefield;
            _loosePanel.gameObject.SetActive(true);
            
            _loosePanel.OnRestartButtonPressed += OnRestartButtonPressed;
            _loosePanel.OnBackMainMenuButtonPressed += OnBackMainMenuButtonPressed;
        }

        public void Exit()
        {
            _loosePanel.OnRestartButtonPressed += OnRestartButtonPressed;
            _loosePanel.OnBackMainMenuButtonPressed += OnBackMainMenuButtonPressed;
        }

        private void OnBackMainMenuButtonPressed() => 
            _gameStateMachine.Enter<BootstrapState>();

        private void OnRestartButtonPressed()
        {
            _gameStateMachine.Enter<LoadingBattleState>();
            _loosePanel.gameObject.SetActive(false);
        }
    }
}