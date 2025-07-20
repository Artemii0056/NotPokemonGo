using LevelSetting;
using UI;

namespace Infrastructure.StateMachines
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