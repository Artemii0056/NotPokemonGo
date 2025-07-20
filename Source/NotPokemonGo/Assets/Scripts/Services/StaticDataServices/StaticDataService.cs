using System.Collections.Generic;
using System.Linq;
using Abilities;
using Characters;
using Characters.Configs;
using Infrastructure;
using LevelSetting;
using Services.AssetManagement;
using Statuses;
using UI;
using UI.SpawnPositions;
using UnityEngine;

namespace Services.StaticDataServices
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IResourceLoader _resourceLoader;

        private Dictionary<AbilityType, AbilityConfig> _abilityConfigs;
        private Dictionary<StatusType, StatusTypeIcon> _statusTypeIcons;
        private Dictionary<SpawnPositionType, SpawnPositionConfig> _spawnPositionConfigs;
        private Dictionary<UnitType, UnitConfig> _unitConfigs;
        private List<LevelConfig> _levelConfigs;
        private Dictionary<int, PlatoonSpawnContainer> _spawnPositionContainer;

        public UnitSkinItemView UnitSkinItemViewPrefab { get; private set; }
        public CharacterSelectionScreenContainer CharacterSelectionScreenContainer { get; private set; }

        public StaticDataService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            LoadAbilityConfigs();
            LoadStatusTypeIcons();
            LoadSpawnPositionConfigs();
            LoadUnitConfigs();
            LoadLevelConfigs();
            LoadUnitSkinItemView();
            LoadCharacterSelectionScreenPanel();
            LoadPlatoonPositionContainer();
        }

        public List<LevelConfig> GetLevelConfigs() => 
            _levelConfigs.ToList();

        public AbilityConfig GetAbilityConfig(AbilityType abilityType)
        {
            if (_abilityConfigs.TryGetValue(abilityType, out AbilityConfig abilityConfig))
                return abilityConfig;

            throw new KeyNotFoundException($"No ability config found for mode {abilityType}");
        }

        public Sprite GetStatusIcon(StatusType statusType)
        {
            if (_statusTypeIcons.TryGetValue(statusType, out StatusTypeIcon statusTypeIcon))
                return statusTypeIcon.Icon;

            throw new KeyNotFoundException($"No ability config found for mode {statusType}");
        }

        public SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType)
        {
            if (_spawnPositionConfigs.TryGetValue(spawnPositionType, out SpawnPositionConfig spawnPositionConfig))
                return spawnPositionConfig;

            throw new KeyNotFoundException($"No ability config found for mode {spawnPositionType}");
        }
        
        public PlatoonSpawnContainer GetSpawnPositionContainer(int count)
        {
            if (_spawnPositionContainer.TryGetValue(count, out PlatoonSpawnContainer platoonSpawnContainer))
                return platoonSpawnContainer;

            throw new KeyNotFoundException($"No ability config found for mode {platoonSpawnContainer}");
        }

        public UnitConfig GetUnitConfig(UnitType unitType)
        {
            if (_unitConfigs.TryGetValue(unitType, out UnitConfig characterConfig))
                return characterConfig;

            throw new KeyNotFoundException($"No character config found for mode {unitType}");
        }

        public void LoadUnitSkinItemView() => 
            UnitSkinItemViewPrefab = _resourceLoader.Load<UnitSkinItemView>(Constants.AssetPath.CharacterSkinItemName);

        public CharactersCatalogStaticData LoadCharacterCatalogStaticDatas() =>
            _resourceLoader.LoadScriptableObject<CharactersCatalogStaticData>(Constants.AssetPath.CatalogPath);
        
       private void LoadCharacterSelectionScreenPanel() =>
           CharacterSelectionScreenContainer = _resourceLoader.Load<CharacterSelectionScreenContainer>(Constants.AssetPath.CharacterSelectionCanvasName);

        private void LoadUnitConfigs() =>
            _unitConfigs = Resources.LoadAll<UnitConfig>(Constants.AssetPath.CharacterConfigsPath)
                .ToDictionary(x => x.Type, x => x);

        private void LoadLevelConfigs() => 
            _levelConfigs = Resources.LoadAll<LevelConfig>(Constants.AssetPath.LevelConfigsPath).ToList();

        private void LoadAbilityConfigs()
        {
            _abilityConfigs = Resources.LoadAll<AbilityConfig>(Constants.AssetPath.AbilityConfigPath)
                .ToDictionary(x => x.AbilityType, x => x);
        }

        private void LoadStatusTypeIcons()
        {
            _statusTypeIcons = Resources.Load<StatusTypesConfig>(Constants.AssetPath.StatusTypePath).StatusTypes
                .ToDictionary(x => x.Type, x => x);
        }

        private void LoadSpawnPositionConfigs()
        {
            _spawnPositionConfigs = Resources.LoadAll<SpawnPositionConfig>(Constants.AssetPath.SpawnPositionConfigsPath)
                .ToDictionary(x => x.SpawnPositionType, x => x);
        }
        private void LoadPlatoonPositionContainer()
        {
            _spawnPositionContainer = Resources.LoadAll<PlatoonSpawnContainer>(Constants.AssetPath.PlatoonContainersPath)
                .ToDictionary(x => x.Count, x => x);
        }
    }
}