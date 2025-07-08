using Infrastructure.StateMachines.States.Interfaces;
using Services.SceneServices;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
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
            _sceneLoader.Load(Constants.AssetPath.CharacterSelectionSceneName, EnterMainMenuState);
        }

        private void EnterMainMenuState() => 
            _gameStateMachine.Enter<LoadingCharacterSelectionState>(); //TODO Начинается все тут

        public void Exit()
        { }
    }
}