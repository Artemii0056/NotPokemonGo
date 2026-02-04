using System.Collections.Generic;
using Abilities;
using Abilities.Configs;
using Characters;
using Characters.Configs;
using DodgeSystem.Configs;
using LevelSetting;
using QteSystem;
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
        QteConfig GetQteConfig(QteType qteType);
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
        List< LevelConfig> GetLevelConfigs();
        UnitSkinItemView UnitSkinItemViewPrefab { get; }
        CharacterSelectionScreenContainer CharacterSelectionScreenContainer { get; }
        CombatText CombatTextPrefab { get; }
        PlatoonSpawnContainer GetSpawnPositionContainer(int count);
        DodgeConfig GetDodgeConfigByUnitType(UnitType sourceUnitType);
    }
}