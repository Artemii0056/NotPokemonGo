using Services.StatesServices;

namespace Infrastructure.StateMachines.GlobalStateMachine
{
    public class GameStateMachine : BaseStateMachine, IGameStateMachine
    {
        public GameStateMachine(IStateProvider stateProvider) : base(stateProvider)
        {
        }
    }
}