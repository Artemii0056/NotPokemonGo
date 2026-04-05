using System.Collections.Generic;
using Abilities;
using AbilityNew.Scripts;
using Characters;
using Characters.Configs;
using LevelSetting;
using QteSystem;
using QteSystem.Configs;
using QteSystem.Core;
using Statuses;
using UI;
using UI.SpawnPositions;
using UnityEngine;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        Sprite GetStatusIcon(StatusType statusType);
        UnitConfig GetUnitConfig(UnitType unitType);
        QteConfig GetQteConfig(QteType qteType);
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
        List< LevelConfig> GetLevelConfigs();
        UnitSkinItemView UnitSkinItemViewPrefab { get; }
        CharacterSelectionScreenContainer CharacterSelectionScreenContainer { get; }
        CombatText.CombatText CombatTextPrefab { get; }
        PlatoonSpawnContainer GetSpawnPositionContainer(int count);
        StatusSetup GetStatusSetup(StatusType statusType);
        ParticleSystem GetParticleByType(StatusType setupType);
        AbilitySo GetCounterattackAbility(UnitType reactorUnitType);
        AbilitySo GetAbilityConfig(AbilityType abilityType);
        AbilitySo GetAbility(UnitType unitType, AbilityType abilityType);
    }
}