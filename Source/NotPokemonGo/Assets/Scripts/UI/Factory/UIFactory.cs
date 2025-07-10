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

        public CharacterSelectionScreenPanel CreateCharacterSelectionScreenPanel(out ChooseUnitsForBattle chooseUnitsForBattle)
        {
            CharacterSelectionScreenPanel screenPanel =
                _resourceLoader.Load<CharacterSelectionScreenPanel>(Constants.AssetPath.CharacterSelectionCanvasName);
            
            CharacterSelectionScreenPanel characterSelectionScreenPanel = Object.Instantiate(screenPanel);
            
            
            ChooseUnitsForBattle unitForBattlePrefab =
                _resourceLoader.Load<ChooseUnitsForBattle>(Constants.AssetPath.ChooseUnitsCanvasName);
            
            ChooseUnitsForBattle unitForBattle = Object.Instantiate(unitForBattlePrefab);

            CharactersCatalogStaticData config = _staticDataService.LoadCharacterCatalogStaticDatas();

            UnitSkinItemView iconPrefab =
                Resources.Load<UnitSkinItemView>(Constants.AssetPath.CharacterSkinItemName);

            foreach (UnitItemConfig characterItemConfig in config.CharacterItemConfigs) //TODO НУЖНО ЭТО ПЕРЕПРОКИНУТЬ В CHOOSE
            {
                UnitSkinItemView icon = Object.Instantiate(iconPrefab);
                icon.InitImage(characterItemConfig);
                
                characterSelectionScreenPanel.UnitContainerPanel.AddItem(icon);
                
                UnitSkinItemView icon2 = Object.Instantiate(iconPrefab);
                icon2.InitImage(characterItemConfig);
                unitForBattle.UnitContainerPanel.AddItem(icon2);
                
                characterSelectionScreenPanel.UnitStatsPanel.SetCharacteristicItemView(
                    CreateCharacteristicItemView());
            }
            
            chooseUnitsForBattle = unitForBattle;
            
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
                
              //  Debug.Log(mapLevel.LevelType);
                
                mapLevels.Add(mapLevel);
                mapLevel.gameObject.SetActive(false);
            }
            
            chooseMapUI.Initialize(mapLevels, _staticDataService); //TODO Вот тут нужно проинициализировать мапы

            CharacterSelectionScreenPanel characterSelectionScreenPanel = CreateCharacterSelectionScreenPanel(out ChooseUnitsForBattle chooseUnitsForBattle);
            
            foreach (var map in mapLevels)
            {
                map.Set(chooseUnitsForBattle);
            }

            startScreen.Initialize(characterSelectionScreenPanel, chooseMapUI);

            return startScreen;
        }

        // public CharacterSelectionScreenPanel CreateCharacterSelectionPanel()
        // {
        //     CharactersCatalogStaticData config = _staticDataService.LoadCharacterCatalogStaticDatas();
        //
        //     Debug.Log(config == null);
        //
        //     UnitSkinItemView iconPrefab =
        //         Resources.Load<UnitSkinItemView>(Constants.AssetPath.CharacterSkinItemName);
        //
        //     CharacterSelectionScreenPanel characterSelectionScreenPanel = CreateCharacterSelectionScreenPanel();
        //
        //     foreach (UnitItemConfig characterItemConfig in config.CharacterItemConfigs)
        //     {
        //         UnitSkinItemView icon = Object.Instantiate(iconPrefab);
        //         icon.InitImage(characterItemConfig);
        //         characterSelectionScreenPanel.UnitContainerPanel.AddItem(icon);
        //         characterSelectionScreenPanel.UnitStatsPanel.SetCharacteristicItemView(
        //             CreateCharacteristicItemView());
        //     }
        //
        //     return characterSelectionScreenPanel;
        // }

        private CharacteristicItemView CreateCharacteristicItemView()
        {
            CharacteristicItemView characteristicItemView =
                _resourceLoader.Load<CharacteristicItemView>(Constants.AssetPath.CharacteristicItemViewPath);

            return characteristicItemView;
        }
    }
}