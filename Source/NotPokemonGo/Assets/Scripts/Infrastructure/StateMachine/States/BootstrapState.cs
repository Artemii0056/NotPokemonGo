using Infrastructure.StateMachine.States.Interfaces;
using Services.SceneServices;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;

        public BootstrapState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader)
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
        { }
    }
}