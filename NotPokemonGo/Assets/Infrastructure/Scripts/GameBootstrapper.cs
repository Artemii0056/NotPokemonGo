using UnityEngine;

namespace Infrastructure.Scripts
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game(this);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);

            string abc = "sdf";
            abc += 'c';
            
            Debug.Log(abc);
        }
    }
}