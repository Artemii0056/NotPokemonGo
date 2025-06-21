using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.States;
using Infrastructure.StateMachine.States.Interfaces;
using Services.AssetManagement;
using Services.StatesServices;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly IStateProvider _stateProvider;
        private IExitableState _currentState;

        public GameStateMachine(IStateProvider stateProvider) => 
            _stateProvider = stateProvider;

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state?.Enter();
        }

        public void Update(float deltaTime)
        {
            if (_currentState is IUpdateState updateState) 
                updateState.Update(deltaTime);
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state?.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            TState state = _stateProvider.GetState<TState>();
            _currentState = state;

            return state;
        }
    }
}