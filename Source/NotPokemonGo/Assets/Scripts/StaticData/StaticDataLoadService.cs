using System.Collections.Generic;
using AssetManagement;
using Characters.Configs;
using Infrastructure;
using StaticData.CharactersCatalog;

namespace StaticData
{
    public class StaticDataLoadService
    {
        private Dictionary<CharacterType, CharacterItemConfig> _charactersDatas;
        private CharactersCatalogStaticData _charactersCatalog;
        private IResourceLoader _resourceLoader;

        public StaticDataLoadService(IResourceLoader resourceLoader) => 
            _resourceLoader = resourceLoader;

        public CharactersCatalogStaticData LoadCharacterCatalogStaticDatas() => 
            _resourceLoader.LoadScriptableObject<CharactersCatalogStaticData>(Constants.AssetPath.CatalogPath);
    }
}