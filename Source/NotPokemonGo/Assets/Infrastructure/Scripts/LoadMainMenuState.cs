namespace Infrastructure.Scripts
{
    public class LoadMainMenuState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadMainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string payload) => 
            _sceneLoader.Load(payload);

        public void Exit()
        {
        }
    }
}