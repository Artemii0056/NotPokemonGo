using Infrastructure.StateMachines.States.Interfaces;
using Services.SceneServices;
using Services.StaticDataServices;
using UI;
using UI.Factory;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class LoadingCharacterSelectionState : IState
    {
        private IStaticDataService _staticDataService;
        private IGameStateMachine _gameStateMachine;
        private ISceneLoader _sceneLoader;
        private IUIFactory _uiFactory;

        public LoadingCharacterSelectionState(IStaticDataService staticDataService, IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _staticDataService = staticDataService;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            StartScreenUI screenUI = _uiFactory.CreateStartScreen();
            // CharacterSelectionScreenPanel characterScreenPanel = _uiFactory.CreateCharacterSelectionScreenPanel();
            // characterScreenPanel.Show();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}