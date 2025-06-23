using Infrastructure.StateMachines.States.Interfaces;
using Services.StatesServices;

namespace Infrastructure.StateMachines
{
    public class BaseStateMachine : IStateMachine
    {
        private readonly IStateProvider _stateProvider;
        private IExitableState _currentState;

        public BaseStateMachine(IStateProvider stateProvider) => 
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