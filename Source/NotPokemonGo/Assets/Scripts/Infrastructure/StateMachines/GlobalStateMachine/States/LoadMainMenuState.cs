using Infrastructure.StateMachines.States.Interfaces;
using Services.SceneServices;
using UI.Factory;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
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

        public void Enter(string unitActionPayload) => 
            _sceneLoader.Load(unitActionPayload, OnLoadMainMenuState);

        public void Exit()
        {
        }
        
        private void OnLoadMainMenuState()
        {
            
        }
    }
}