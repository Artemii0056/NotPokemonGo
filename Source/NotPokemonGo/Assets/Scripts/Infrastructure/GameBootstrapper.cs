using Infrastructure.Scripts.AssetManagement;
using Infrastructure.Scripts.Services;
using Infrastructure.Scripts.StateMachine.States;
using Source.StaticData;
using UnityEngine;

namespace Infrastructure.Scripts
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, new StaticDataLoadService(new ResourceLoader()));
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}