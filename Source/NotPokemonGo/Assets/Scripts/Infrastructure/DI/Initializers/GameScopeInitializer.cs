using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers
{
    public class GameScopeInitializer : MonoBehaviour, IInitializable
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