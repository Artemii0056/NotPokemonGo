using Infrastructure.StateMachine.States.Interfaces;
using Services.SceneServices;
using UI;
using UI.Factory;

namespace Infrastructure.StateMachine.States
{
    public class LoadMainMenuState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        public LoadMainMenuState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void Enter(string payload) => 
            _sceneLoader.Load(payload, OnLoadMainMenuState);

        public void Exit()
        {
        }
        
        private void OnLoadMainMenuState()
        {
            MainMenuUI mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.gameObject.SetActive(false);
            CharacterSelectionScreenPanel selectionScreenPanel = _uiFactory.CreateCharacterSelectionPanel();
             //selectionScreenPanel.Show();
            // selectionPanel.gameObject.SetActive(false);
        }
    }
}