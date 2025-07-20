using Infrastructure.StateMachines.States.Interfaces;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class StartScreenState : IPayloadedState<StartMenuPayload>, IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private StartScreenUI _startScreenUI;

        public StartScreenState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter(StartMenuPayload payload)
        {
            _startScreenUI = payload.UI;
            _startScreenUI.gameObject.SetActive(true);
            
            _startScreenUI.ShowHeroesClicked += OnShowHeroesClicked;
            _startScreenUI.ChoosePlatoonClicked += OnChoosePlatoonClicked;
        }
        
        public void Enter()
        {
            _startScreenUI.gameObject.SetActive(true);
            
            _startScreenUI.ShowHeroesClicked += OnShowHeroesClicked;
            _startScreenUI.ChoosePlatoonClicked += OnChoosePlatoonClicked;
        }

        private void OnChoosePlatoonClicked()
        {
            ChoosePlatoonPayload payload = new ChoosePlatoonPayload(_startScreenUI.ChooseMapUI, _gameStateMachine);
            
            _gameStateMachine.Enter<ChooseMapState, ChoosePlatoonPayload>(payload);
        }

        private void OnShowHeroesClicked()
        {
            ShowHeroPayload payload = new ShowHeroPayload(_startScreenUI.CharacterSelectionScreenContainer);
            
            _gameStateMachine.Enter<ShowHeroState, ShowHeroPayload>(payload);
        }

        public void Exit()
        {
            _startScreenUI.gameObject.SetActive(false);
            
            _startScreenUI.ShowHeroesClicked -= OnShowHeroesClicked;
            _startScreenUI.ChoosePlatoonClicked -= OnChoosePlatoonClicked;
        }
    }
}