using System.Collections.Generic;
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

        public UnitSelectionController CreateUnitSelectionController(IEnumerable<UnitItemConfig> configCharacterItemConfigs)
        {
            UnitSelectionController unitForBattlePrefab =
                _resourceLoader.Load<UnitSelectionController>(Constants.AssetPath.ChooseUnitsCanvasName);

            UnitSelectionController unitSelectionController = Object.Instantiate(unitForBattlePrefab);

            unitSelectionController.UnitContainerPanel.AddItems(CreateUnitSkinItemViews(configCharacterItemConfigs));

            return unitSelectionController;
        }

        public List<UnitSkinItemView> CreateUnitSkinItemViews(IEnumerable<UnitItemConfig> configCharacterItemConfigs)
        {
            List<UnitSkinItemView> unitSkinItemViews = new List<UnitSkinItemView>();

            foreach (var config in configCharacterItemConfigs)
            {
                UnitSkinItemView unitSkinItemView2 = CreateUnitSkinItemView();
                unitSkinItemView2.InitImage(config);
                unitSkinItemViews.Add(unitSkinItemView2);
            }

            return unitSkinItemViews;
        }

        public UnitSkinItemView CreateUnitSkinItemView() =>
            Object.Instantiate(_staticDataService.UnitSkinItemViewPrefab);

        public CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel()
        {
            CharacterSelectionScreenPanel characterSelectionScreenPanel =
                Object.Instantiate(_staticDataService.CharacterSelectionScreenPanel);

            CharactersCatalogStaticData config = _staticDataService.LoadCharacterCatalogStaticDatas();

            characterSelectionScreenPanel.UnitContainerPanel.AddItems(CreateUnitSkinItemViews(config.CharacterItemConfigs));

            characterSelectionScreenPanel.UnitStatsPanel.SetCharacteristicItemView(CreateCharacteristicItemView()); 

            return characterSelectionScreenPanel;
        }

        public MainMenuUI CreateMainMenu()
        {
            MainMenuUI menu = _resourceLoader.Load<MainMenuUI>(Constants.AssetPath.MainMenuCanvasPath);

            return Object.Instantiate(menu);
        }

        public StartScreenUI CreateStartScreen()
        {
            StartScreenUI startScreenPrefab =
                _resourceLoader.Load<StartScreenUI>(Constants.AssetPath.StartScreenCanvasName);

            ChooseMapUI chooseMapUIPrefab = _resourceLoader.Load<ChooseMapUI>(Constants.AssetPath.ChooseMapCanvasName);

            StartScreenUI startScreen = Object.Instantiate(startScreenPrefab);
            ChooseMapUI chooseMapUI = Object.Instantiate(chooseMapUIPrefab);

            MapLevel[] mapsResources = Resources.LoadAll<MapLevel>("Maps");

            List<MapLevel> mapLevels = new List<MapLevel>();

            foreach (var map in mapsResources)
            {
                MapLevel mapLevel = Object.Instantiate(map);

                mapLevels.Add(mapLevel);
                mapLevel.gameObject.SetActive(false);
            }

            chooseMapUI.Initialize(mapLevels, _staticDataService); 

            CharacterSelectionScreenPanel characterSelectionScreenPanel =
                CreateCharacterSelectionScreenPanel();
            
            CharactersCatalogStaticData config = _staticDataService.LoadCharacterCatalogStaticDatas();
            
            UnitSelectionController selectionController = CreateUnitSelectionController(config.CharacterItemConfigs);

            foreach (var map in mapLevels)
            {
                map.Set(selectionController);
            }

            startScreen.Initialize(characterSelectionScreenPanel, chooseMapUI);

            return startScreen;
        }

        private CharacteristicItemView CreateCharacteristicItemView()
        {
            CharacteristicItemView characteristicItemView =
                _resourceLoader.Load<CharacteristicItemView>(Constants.AssetPath.CharacteristicItemViewPath);

            return characteristicItemView;
        }
    }
}