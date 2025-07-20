using System.Collections.Generic;
using Abilities;
using Characters;
using Characters.Configs;
using LevelSetting;
using Statuses;
using UI;
using UI.SpawnPositions;
using UnityEngine;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType);
        AbilityConfig GetAbilityConfig(AbilityType abilityType);
        Sprite GetStatusIcon(StatusType statusType);
        UnitConfig GetUnitConfig(UnitType unitType);
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
        List< LevelConfig> GetLevelConfigs();
        UnitSkinItemView UnitSkinItemViewPrefab { get; }
        CharacterSelectionScreenContainer CharacterSelectionScreenContainer { get; }
        PlatoonSpawnContainer GetSpawnPositionContainer(int count);
    }
}