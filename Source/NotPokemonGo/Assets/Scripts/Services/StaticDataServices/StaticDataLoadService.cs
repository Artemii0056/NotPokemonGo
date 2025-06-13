using System.Collections.Generic;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;

namespace Services.StaticDataServices
{
    public class StaticDataLoadService : IStaticDataLoadService
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