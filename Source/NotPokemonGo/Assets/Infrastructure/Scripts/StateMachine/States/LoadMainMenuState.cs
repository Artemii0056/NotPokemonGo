using Source.StaticData;
using Source.UI.Factory;

namespace Infrastructure.Scripts.StateMachine.States
{
    public class LoadMainMenuState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private StaticDataLoadService _staticDataLoadService;
        private UIFactory _uiFactory;

        public LoadMainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, StaticDataLoadService staticDataLoadService, UIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _staticDataLoadService = staticDataLoadService;
            _uiFactory = uiFactory;
        }

        public void Enter(string payload) => 
            _sceneLoader.Load(payload, OnLoadMainMenuState);

        public void Exit()
        {
        }
        
        private void OnLoadMainMenuState()
        {
            _uiFactory.CreateShop();
            _uiFactory.CreateMainMenu();
           //Подгрузка конфигов готова, надо тестить
        }
    }
}