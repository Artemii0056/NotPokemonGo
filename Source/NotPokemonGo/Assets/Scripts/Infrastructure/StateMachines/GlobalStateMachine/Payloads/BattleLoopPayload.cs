using UI.Ability;

namespace Infrastructure.StateMachines.GlobalStateMachine.Payloads
{
    public class BattleLoopPayload
    {
        public BattleLoopPayload(AbilitiesPanel abilitiesPanel, Battlefield battlefield)
        {
            AbilitiesPanel = abilitiesPanel;
            Battlefield = battlefield;
        }

        public Battlefield Battlefield { get; private set; }
        public AbilitiesPanel AbilitiesPanel { get; private set; }
    }
}