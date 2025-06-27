using UI.Ability;

namespace Infrastructure.StateMachines.GlobalStateMachine.Payloads
{
    public class BattleLoopPayload
    {
        public BattleLoopPayload( Battlefield battlefield)
        {
            Battlefield = battlefield;
        }

        public Battlefield Battlefield { get; private set; }
    }
}