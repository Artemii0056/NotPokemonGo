using System.Data.Common;
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

        public CharacterSelectionScreenPanel CreateShop()
        {
            CharacterSelectionScreenPanel shop = _resourceLoader.Load<CharacterSelectionScreenPanel>(Constants.AssetPath.CharacterSelectionCanvasName);
            return Object.Instantiate(shop);
        }

        public MainMenuUI CreateMainMenu()
        {
            MainMenuUI menu = _resourceLoader.Load<MainMenuUI>(Constants.AssetPath.MainMenuCanvasPath);
            return Object.Instantiate(menu);
        }

        public CharacterSelectionScreenPanel CreateCharacterSelectionPanel()
        {
            CharactersCatalogStaticData config = _staticDataLoadService.LoadCharacterCatalogStaticDatas();
            
            CharacterSkinItemView iconPrefab = Resources.Load<CharacterSkinItemView>(Constants.AssetPath.CharacterSkinItemName);

            CharacterSelectionScreenPanel shop = CreateShop();

            foreach (var conf in config.CharacterItemConfigs)
            {
                CharacterSkinItemView icon = Object.Instantiate(iconPrefab);
                icon.InitImage(conf.Sprite);
                shop.CharacterSelectionPanel.AddItem(icon);
            }

            return shop;
        }
    }
}