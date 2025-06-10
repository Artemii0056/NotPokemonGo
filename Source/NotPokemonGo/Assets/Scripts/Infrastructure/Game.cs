using Infrastructure.StateMachine;
using Services;
using StaticData;

namespace Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;
    
        public Game(ICoroutineRunner coroutineRunner, StaticDataLoadService staticDataLoadService)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), staticDataLoadService);
        }
    }
}