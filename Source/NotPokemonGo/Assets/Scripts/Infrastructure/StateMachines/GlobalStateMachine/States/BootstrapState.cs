using Infrastructure.StateMachines.States.Interfaces;
using Services.SceneServices;
using UI;
using UI.Factory;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        public BootstrapState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.AssetPath.CharacterSelectionSceneName, EnterMainMenuState);
        }

        private void EnterMainMenuState()
        {
            StartScreenUI screenUI = _uiFactory.CreateStartScreen();
            
            StartMenuPayload payload = new StartMenuPayload(screenUI);
            _gameStateMachine.Enter<StartScreenState, StartMenuPayload>(payload); 
        }

        public void Exit()
        {
        }
    }
}