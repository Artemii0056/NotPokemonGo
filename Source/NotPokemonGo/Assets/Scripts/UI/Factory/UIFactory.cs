using Characters;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;
using Services.StaticDataServices;
using UnityEngine;

namespace UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IStaticDataService _staticDataService;

        public UIFactory(IResourceLoader resourceLoader,
            IStaticDataService staticDataService)
        {
            _resourceLoader = resourceLoader;
            _staticDataService = staticDataService;
        }

        public CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel()
        {
            CharacterSelectionScreenPanel shop =
                _resourceLoader.Load<CharacterSelectionScreenPanel>(Constants.AssetPath.CharacterSelectionCanvasName);
            
            CharacterSelectionScreenPanel characterSelectionScreenPanel = Object.Instantiate(shop);
            
            CharactersCatalogStaticData config = _staticDataService.LoadCharacterCatalogStaticDatas();
            
            UnitSkinItemView iconPrefab =
                Resources.Load<UnitSkinItemView>(Constants.AssetPath.CharacterSkinItemName);
            
            foreach (UnitItemConfig characterItemConfig in config.CharacterItemConfigs)
            {
                UnitSkinItemView icon = Object.Instantiate(iconPrefab);
                icon.InitImage(characterItemConfig);
                characterSelectionScreenPanel.UnitContainerPanel.AddItem(icon);
                characterSelectionScreenPanel.UnitStatsPanel.SetCharacteristicItemView(
                    CreateCharacteristicItemView());
            }
            
            return characterSelectionScreenPanel;
        }

        public MainMenuUI CreateMainMenu()
        {
            MainMenuUI menu = _resourceLoader.Load<MainMenuUI>(Constants.AssetPath.MainMenuCanvasPath);
            
            return Object.Instantiate(menu);
        }

        public CharacterSelectionScreenPanel CreateCharacterSelectionPanel()
        {
            CharactersCatalogStaticData config = _staticDataService.LoadCharacterCatalogStaticDatas();
            
            Debug.Log(config == null);
            
            UnitSkinItemView iconPrefab =
                Resources.Load<UnitSkinItemView>(Constants.AssetPath.CharacterSkinItemName);
            
            CharacterSelectionScreenPanel characterSelectionScreenPanel = CreateCharacterSelectionScreenPanel();
            
            foreach (UnitItemConfig characterItemConfig in config.CharacterItemConfigs)
            {
                UnitSkinItemView icon = Object.Instantiate(iconPrefab);
                icon.InitImage(characterItemConfig);
                characterSelectionScreenPanel.UnitContainerPanel.AddItem(icon);
                characterSelectionScreenPanel.UnitStatsPanel.SetCharacteristicItemView(
                    CreateCharacteristicItemView());
            }
            
            return characterSelectionScreenPanel;
        }

        private CharacteristicItemView CreateCharacteristicItemView()
        {
            CharacteristicItemView characteristicItemView =
                _resourceLoader.Load<CharacteristicItemView>(Constants.AssetPath.CharacteristicItemViewPath);
            
            return characteristicItemView;
        }
    }
}