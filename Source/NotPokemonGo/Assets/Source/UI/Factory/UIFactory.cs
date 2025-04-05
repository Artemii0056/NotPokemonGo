using Infrastructure;
using Infrastructure.Scripts.AssetManagement;
using Source.StaticData;
using Source.StaticData.CharactersCatalog;
using Source.StaticData.CharactersCatalog.Scripts;
using Source.UI.Scripts;
using UnityEngine;

namespace Source.UI.Factory
{
    public class UIFactory
    {
        private IResourceLoader _resourceLoader;
        private StaticDataLoadService _staticDataLoadService;

        public UIFactory(IResourceLoader resourceLoader, StaticDataLoadService staticDataLoadService)
        {
            _resourceLoader = resourceLoader;
            _staticDataLoadService = staticDataLoadService;
        }

        public CharacterSelectionPanel CreateShop()
        {
            CharacterSelectionPanel shop = _resourceLoader.Load<CharacterSelectionPanel>(Constants.AssetPath.CharacterSelectionCanvasName);
            return Object.Instantiate(shop);
        }

        public MainMenuUI CreateMainMenu()
        {
            MainMenuUI menu = _resourceLoader.Load<MainMenuUI>(Constants.AssetPath.MainMenuCanvasPath);
            return Object.Instantiate(menu);
        }

        public CharacterSelectionPanel CreateCharacterSelectionPanel()
        {
            CharactersCatalogStaticData config = _staticDataLoadService.LoadCharacterCatalogStaticDatas();
            
            CharacterSkinItem iconPrefab = Resources.Load<CharacterSkinItem>(Constants.AssetPath.CharacterSkinItemName);

            CharacterSelectionPanel shop = CreateShop();

            foreach (var conf in config.Characters)
            {
                CharacterSkinItem icon = Object.Instantiate(iconPrefab);
                icon.InitImage(conf.Sprite);
                shop.AddItem(icon);
            }

            return shop;
        }
    }
}