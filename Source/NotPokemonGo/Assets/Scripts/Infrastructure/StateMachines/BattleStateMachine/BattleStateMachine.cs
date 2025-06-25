using Services.StatesServices;

namespace Infrastructure.StateMachines.BattleStateMachine
{
    public class BattleStateMachine :  BaseStateMachine, IBattleStateMachine
    {
        public BattleStateMachine(IStateProvider stateProvider) : base(stateProvider)
        {
        }
    }
}