using System.Collections.Generic;
using Battlefields;
using Characters.Configs;
using LevelSetting;
using Platoons;
using UnityEngine;

namespace Services.BattleUnitContainers
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(List<UnitType> units, LevelPartSetup levelPartSetup, out GameObject battlefieldGameObject);
        Battlefield Create(List<UnitType> units, Platoon friendPlatoon, LevelPartSetup levelPartSetup, out GameObject battlefieldGameObject);
    }
}