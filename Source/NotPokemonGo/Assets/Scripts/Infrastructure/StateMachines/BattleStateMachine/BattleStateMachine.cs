using Services.StatesServices;

namespace Infrastructure.StateMachines.BattleStateMachine
{
    public class BattleStateMachine : BaseStateMachine, IBattleStateMachine
    {
        public BattleStateMachine(IStateFactory stateFactory) : base(stateFactory)
        {
        }
    }
}