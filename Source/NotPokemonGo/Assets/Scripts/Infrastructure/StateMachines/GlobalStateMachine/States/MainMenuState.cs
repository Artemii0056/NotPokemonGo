using Infrastructure.StateMachines.States.Interfaces;
using Services.SceneServices;
using UI.Factory;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class MainMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        public MainMenuState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
        }
    }
}