using Services.StatesServices;

namespace Infrastructure.StateMachines.GlobalStateMachine
{
    public class GameStateMachine : BaseStateMachine, IGameStateMachine
    {
        public GameStateMachine(IStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}