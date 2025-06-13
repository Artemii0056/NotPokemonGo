using Characters;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;
using Services.StaticDataServices;
using UnityEngine;

namespace UI.Factory
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

        public CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel()
        {
            CharacterSelectionScreenPanel shop = _resourceLoader.Load<CharacterSelectionScreenPanel>(Constants.AssetPath.CharacterSelectionCanvasName);
            return Object.Instantiate(shop);
        }

        public MainMenuUI CreateMainMenu()
        {
            MainMenuUI menu = _resourceLoader.Load<MainMenuUI>(Constants.AssetPath.MainMenuCanvasPath);
            return Object.Instantiate(menu);
        }

        public CharacteristicItemView CreateCharacteristicItemView()
        {
            CharacteristicItemView characteristicItemView =  _resourceLoader.Load<CharacteristicItemView>(Constants.AssetPath.CharacteristicItemViewPath);
            return characteristicItemView;
        }

        public CharacterSelectionScreenPanel CreateCharacterSelectionPanel()
        {
            CharactersCatalogStaticData config = _staticDataLoadService.LoadCharacterCatalogStaticDatas();
            
            CharacterSkinItemView iconPrefab = Resources.Load<CharacterSkinItemView>(Constants.AssetPath.CharacterSkinItemName);

            CharacterSelectionScreenPanel characterSelectionScreenPanel = CreateCharacterSelectionScreenPanel();

            foreach (var conf in config.CharacterItemConfigs)
            {
                CharacterSkinItemView icon = Object.Instantiate(iconPrefab);
                icon.InitImage(conf);
                characterSelectionScreenPanel.CharacterSelectionPanel.AddItem(icon);
                characterSelectionScreenPanel.CharacterInfoPanel.SetCharacteristicItemView(CreateCharacteristicItemView());
            }

            return characterSelectionScreenPanel;
        }
    }
}