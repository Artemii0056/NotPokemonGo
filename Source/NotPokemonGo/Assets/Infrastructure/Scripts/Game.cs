using Infrastructure.Scripts.Services;
using Infrastructure.Scripts.StateMachine;
using Source.StaticData;

namespace Infrastructure.Scripts
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