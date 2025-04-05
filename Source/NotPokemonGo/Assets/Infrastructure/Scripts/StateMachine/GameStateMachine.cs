using System;
using System.Collections.Generic;
using Infrastructure.Scripts.AssetManagement;
using Infrastructure.Scripts.StateMachine.States;
using Source.StaticData;
using Source.UI.Factory;

namespace Infrastructure.Scripts.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;
        private IResourceLoader _resourceLoader;

        public GameStateMachine(SceneLoader sceneLoader, StaticDataLoadService staticDataLoadService)
        {
            _resourceLoader = new ResourceLoader();

            var uiFactory = new UIFactory(_resourceLoader, staticDataLoadService);

            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadMainMenuState)] =
                    new LoadMainMenuState(this, sceneLoader, uiFactory),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state?.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state?.Enter(payload);
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}