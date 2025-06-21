using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers.Globals
{
    public class GameScopeInitializer : MonoBehaviour, IInitializable, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Initialize()
        {
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}