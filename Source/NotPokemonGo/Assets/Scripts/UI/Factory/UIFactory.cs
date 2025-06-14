using Characters;
using Characters.Configs;
using Infrastructure;
using Services.AssetManagement;
using Services.StaticDataServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IStaticDataLoadService _staticDataLoadService;

        public UIFactory(IResourceLoader resourceLoader,
            IStaticDataLoadService staticDataLoadService)
        {
            _resourceLoader = resourceLoader;
            _staticDataLoadService = staticDataLoadService;
        }

        public CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel()
        {
            // CharacterSelectionScreenPanel shop =
            //     _resourceLoader.Load<CharacterSelectionScreenPanel>(Constants.AssetPath.CharacterSelectionCanvasName);
            //
            // CharacterSelectionScreenPanel characterSelectionScreenPanel = Object.Instantiate(shop);
            // // _objectResolver.Inject(characterSelectionScreenPanel);
            //
            // return characterSelectionScreenPanel;

            return default;
        }

        public MainMenuUI CreateMainMenu()
        {
            // MainMenuUI menu = _resourceLoader.Load<MainMenuUI>(Constants.AssetPath.MainMenuCanvasPath);
            //
            // // _objectResolver.Inject(menu);
            //
            // return Object.Instantiate(menu);
            return default;
        }

        public CharacterSelectionScreenPanel CreateCharacterSelectionPanel()
        {
            // CharactersCatalogStaticData config = _staticDataLoadService.LoadCharacterCatalogStaticDatas();
            //
            // CharacterSkinItemView iconPrefab =
            //     Resources.Load<CharacterSkinItemView>(Constants.AssetPath.CharacterSkinItemName);
            //
            // CharacterSelectionScreenPanel characterSelectionScreenPanel = CreateCharacterSelectionScreenPanel();
            //
            // foreach (CharacterItemConfig characterItemConfig in config.CharacterItemConfigs)
            // {
            //     CharacterSkinItemView icon = Object.Instantiate(iconPrefab);
            //     icon.InitImage(characterItemConfig);
            //     characterSelectionScreenPanel.CharacterSelectionPanel.AddItem(icon);
            //     characterSelectionScreenPanel.CharacterInfoPanel.SetCharacteristicItemView(
            //         CreateCharacteristicItemView());
            // }
            //
            // return characterSelectionScreenPanel;
            return default;
        }

        private CharacteristicItemView CreateCharacteristicItemView()
        {
            // CharacteristicItemView characteristicItemView =
            //     _resourceLoader.Load<CharacteristicItemView>(Constants.AssetPath.CharacteristicItemViewPath);
            //
            // return characteristicItemView;
            return default;
        }
    }
}