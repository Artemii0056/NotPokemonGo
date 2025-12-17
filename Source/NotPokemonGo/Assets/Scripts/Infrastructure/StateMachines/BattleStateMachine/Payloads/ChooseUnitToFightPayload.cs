using LevelSetting;
using UI;

namespace Infrastructure.StateMachines.BattleStateMachine.Payloads
{
    public class ChooseUnitToFightPayload
    {
        public readonly ChooseUnitToFightPanel ChooseUnitToFightPanel;
        public readonly LevelConfig Config;

        public ChooseUnitToFightPayload(ChooseUnitToFightPanel characterSelectionScreenPanel, LevelConfig config)
        {
            ChooseUnitToFightPanel = characterSelectionScreenPanel;
            Config = config;
        }
    }
}