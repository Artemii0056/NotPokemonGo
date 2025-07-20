using System.Collections.Generic;
using Abilities;
using Characters;
using Characters.Configs;
using QTESystem;
using Services.QTEServices;
using Statuses;
using UnityEngine;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType);
        AbilityConfig GetAbilityConfig(AbilityType abilityType);
        Sprite GetStatusIcon(StatusType statusType);
        UnitConfig GetUnitConfig(UnitType unitType);
        List<AbilityConfig> GetAllAbilityConfigs();
        QTEConfig GetQTEConfig(QTEType qteMode);
    }
}