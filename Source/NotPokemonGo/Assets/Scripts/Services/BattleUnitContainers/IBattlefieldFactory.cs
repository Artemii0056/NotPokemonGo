using System.Collections.Generic;
using Characters.Configs;
using Infrastructure.StateMachines;
using Infrastructure.StateMachines.GlobalStateMachine;
using LevelSetting;

namespace Services.BattleUnitContainers
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(List<UnitType> units, LevelPartSetup levelPartSetup);
    }
}