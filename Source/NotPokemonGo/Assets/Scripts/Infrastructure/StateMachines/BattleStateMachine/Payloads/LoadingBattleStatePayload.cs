using System.Collections.Generic;
using Characters.Configs;
using LevelSetting;

namespace Infrastructure.StateMachines
{
    public class LoadingBattleStatePayload
    {
        public readonly List<UnitType> UnitTypes;
        public readonly LevelConfig LevelConfig;

        public LoadingBattleStatePayload(List<UnitType> unitTypes, LevelConfig levelConfig)
        {
            UnitTypes = unitTypes;
            LevelConfig = levelConfig;
        }
    }
}