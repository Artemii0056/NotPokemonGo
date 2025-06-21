using System.Collections.Generic;
using System.Linq;
using Abilities;
using Characters;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;
using UnityEngine;

namespace Services.StaticDataServices
{
    public class StaticDataService : IStaticDataService
    {
        private IResourceLoader _resourceLoader;
        private Dictionary<TargetMode, AbilityConfig> _abilityConfigs;
        private Dictionary<SpawnPositionType, SpawnPositionConfig> _locationTypeConfigs;

        public StaticDataService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            LoadAbilityConfigs();
            LoadSpawnPositionConfigs();
        }

        public CharactersCatalogStaticData LoadCharacterCatalogStaticDatas() =>
            _resourceLoader.LoadScriptableObject<CharactersCatalogStaticData>(Constants.AssetPath.CatalogPath);

        public AbilityConfig GetAbilityConfig(TargetMode mode)
        {
            if (_abilityConfigs.TryGetValue(mode, out AbilityConfig abilityConfig))
                return abilityConfig;

            throw new KeyNotFoundException($"No ability config found for mode {mode}");
        }
        
        public SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType)
        {
            if (_locationTypeConfigs.TryGetValue(spawnPositionType, out  SpawnPositionConfig locationTypeConfig))
                return locationTypeConfig;
            
            throw new KeyNotFoundException($"No location type config found for {spawnPositionType}");
        }

        private void LoadSpawnPositionConfigs()
        {
             _locationTypeConfigs = Resources.LoadAll<SpawnPositionConfig>("SpawnPositions")
                 .ToDictionary(x => x.SpawnPositionType, x => x);
        }
// 
        private void LoadAbilityConfigs()
        {
            // _abilityConfigs = Resources.LoadAll<AbilityConfig>("Abilities")
            //     .ToDictionary(x => x.TargetMode, x => x);
        }
    }
}