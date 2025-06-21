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
    public class StaticDataService : IStaticDataService
    {
        private readonly IResourceLoader _resourceLoader;

        private Dictionary<AbilityType, AbilityConfig> _abilityConfigs;
        private Dictionary<StatusType, StatusTypeIcon> _statusTypeIcons;
        private Dictionary<SpawnPositionType, SpawnPositionConfig> _spawnPositionConfigs;
        private Dictionary<CharacterType, CharacterConfig> _characterConfigs;

        public StaticDataService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            LoadAbilityConfigs();
            LoadStatusTypeIcons();
            LoadSpawnPositionConfigs();
            LoadCharacterConfigs();
        }

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
        
        public CharacterConfig GetCharacterConfig(CharacterType characterType)
        {
            if (_characterConfigs.TryGetValue(characterType, out CharacterConfig characterConfig))
                return characterConfig;

            throw new KeyNotFoundException($"No character config found for mode {characterType}");
        }

        private void LoadCharacterConfigs()
        {
            _characterConfigs = Resources.LoadAll<CharacterConfig>(Constants.AssetPath.CharacterConfigsPath)
                .ToDictionary(x => x.Type, x => x);
        }

        private CharactersCatalogStaticData LoadCharacterCatalogStaticDatas() =>
            _resourceLoader.LoadScriptableObject<CharactersCatalogStaticData>(Constants.AssetPath.CatalogPath);

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
            Debug.Log("Loading spawn position configs");
            _spawnPositionConfigs = Resources.LoadAll<SpawnPositionConfig>(Constants.AssetPath.SpawnPositionConfigsPath)
                .ToDictionary(x => x.SpawnPositionType, x => x);
        }
    }
}