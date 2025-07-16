using Infrastructure.StateMachines.States.Interfaces;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class ShowHeroState : IPayloadedState<ShowHeroPayload>
    {
        private IGameStateMachine _gameStateMachine;
        private CharacterSelectionScreenPanel _characterSelectionScreenPanel;

        public void Enter(ShowHeroPayload payload)
        {
            _gameStateMachine = payload.GameStateMachine;
            _characterSelectionScreenPanel = payload.CharacterSelectionScreenPanel;

            _characterSelectionScreenPanel.gameObject.SetActive(true);
            _characterSelectionScreenPanel.Show();

            _characterSelectionScreenPanel.ExitClicked += OnExitClicked;
        }

        private void OnExitClicked()
        {
            _gameStateMachine.Enter<StartScreenState>();
        }

        public void Exit()
        {
            _characterSelectionScreenPanel.ExitClicked -= OnExitClicked;
            _characterSelectionScreenPanel.gameObject.SetActive(false);
        }
    }
}