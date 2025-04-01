using UnityEngine;

namespace Infrastructure.Scripts
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
            _sceneLoader.Load(Constants.InitialSceneName, EnterMainMenuState);
        }

        private void EnterMainMenuState() => 
            _gameStateMachine.Enter<LoadMainMenuState, string>("MainMenu");

        public void Exit()
        {
            
        }
    }
}