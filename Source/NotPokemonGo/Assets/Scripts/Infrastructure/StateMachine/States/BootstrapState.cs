using Infrastructure.StateMachine.States.Interfaces;
using Services.SceneServices;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.AssetPath.InitialSceneName, EnterMainMenuState);
        }

        private void EnterMainMenuState() => 
            _gameStateMachine.Enter<LoadMainMenuState, string>(Constants.AssetPath.MainMenuSceneName);

        public void Exit()
        {
            
        }
    }
}