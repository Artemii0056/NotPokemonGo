using System.Collections.Generic;
using System.Linq;
using Abilities;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;
using UnityEngine;

namespace Services.StaticDataServices
{
    public class StaticDataLoadService : IStaticDataLoadService
    {
        private IResourceLoader _resourceLoader;
        private Dictionary<AbilityType, AbilityConfig> _abilityConfigs;

        public StaticDataLoadService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            //LoadAbilityConfigs();
        }
        
        public StaticDataLoadService()
        {
            LoadAbilityConfigs();
        }

        public CharactersCatalogStaticData LoadCharacterCatalogStaticDatas() =>
            _resourceLoader.LoadScriptableObject<CharactersCatalogStaticData>(Constants.AssetPath.CatalogPath);

        public AbilityConfig GetAbilityConfig(AbilityType abilityType)
        {
            if (_abilityConfigs.TryGetValue(abilityType, out AbilityConfig abilityConfig))
                return abilityConfig;
            
            throw new KeyNotFoundException($"No ability config found for mode {abilityType}");
        }

        private void LoadAbilityConfigs()
        {
            _abilityConfigs = Resources.LoadAll<AbilityConfig>(Constants.AssetPath.AbilityConfigPath)
                .ToDictionary(x => x.AbilityType, x => x);
        }
    }
}