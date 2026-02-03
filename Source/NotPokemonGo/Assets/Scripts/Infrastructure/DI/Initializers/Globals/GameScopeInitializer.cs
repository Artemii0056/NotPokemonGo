using System;
using Infrastructure.StateMachines.GlobalStateMachine;
using Infrastructure.StateMachines.GlobalStateMachine.States;
using Services;
using Services.EffectViewServices;
using Services.InputServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers.Globals
{
    public class GameScopeInitializer : MonoBehaviour, IInitializable, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;
        private IInputReader _inputReader;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, EffectViewService effectViewService)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Initialize()
        {
            _gameStateMachine.Enter<BootstrapState>();
        }

        public void Update()
        {
            _gameStateMachine.Update(Time.deltaTime);
        }
    }
}