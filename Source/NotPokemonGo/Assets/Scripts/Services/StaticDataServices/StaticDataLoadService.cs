using System.Collections.Generic;
using System.Linq;
using Abilities;
using Characters;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;
using Statuses;
using UnityEngine;

namespace Services.StaticDataServices
{
    public class StaticDataLoadService : IStaticDataLoadService
    {
        private readonly IResourceLoader _resourceLoader;

        private Dictionary<AbilityType, AbilityConfig> _abilityConfigs;
        private Dictionary<StatusType, StatusTypeIcon> _statusTypeIcons;
        private Dictionary<SpawnPositionType, SpawnPositionConfig> _spawnPositionConfigs;

        public StaticDataLoadService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            //LoadAbilityConfigs();
        }

        public StaticDataLoadService()
        {
            LoadAbilityConfigs();
            LoadStatusTypeIcons();
            LoadSpawnPositionConfigs();
        }

        public CharactersCatalogStaticData LoadCharacterCatalogStaticDatas() =>
            _resourceLoader.LoadScriptableObject<CharactersCatalogStaticData>(Constants.AssetPath.CatalogPath);

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
    }

    public interface IStaticDataLoadService
    {
        AbilityConfig GetAbilityConfig(AbilityType abilityType);
        Sprite GetStatusIcon(StatusType statusType);
        SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType);
    }
}