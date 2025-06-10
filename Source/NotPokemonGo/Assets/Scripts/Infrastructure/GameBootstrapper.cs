using AssetManagement;
using Infrastructure.StateMachine.States;
using Services;
using StaticData;
using UnityEngine;

namespace Infrastructure
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