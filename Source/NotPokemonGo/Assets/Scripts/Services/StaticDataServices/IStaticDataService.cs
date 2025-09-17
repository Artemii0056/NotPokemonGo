using System.Collections.Generic;
using Abilities;
using Characters;
using Characters.Configs;
using LevelSetting;
using QTESystem;
using Statuses;
using UI;
using UI.SpawnPositions;
using UnityEngine;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        AbilityConfig GetAbilityConfig(AbilityType abilityType);
        Sprite GetStatusIcon(StatusType statusType);
        UnitConfig GetUnitConfig(UnitType unitType);
        List<AbilityConfig> GetAllAbilityConfigs();
        QTEConfig GetQTEConfig(AbilityType abilityType);
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
        List< LevelConfig> GetLevelConfigs();
        UnitSkinItemView UnitSkinItemViewPrefab { get; }
        CharacterSelectionScreenContainer CharacterSelectionScreenContainer { get; }
        PlatoonSpawnContainer GetSpawnPositionContainer(int count);
    }
}