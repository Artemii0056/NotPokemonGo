using UI.Ability;

namespace Infrastructure.StateMachines.States.Interfaces
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