using System.Collections.Generic;
using Characters.Configs;
using LevelSetting;

namespace Services.BattleUnitContainers
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(List<UnitType> units, LevelConfig levelConfig);
    }
}