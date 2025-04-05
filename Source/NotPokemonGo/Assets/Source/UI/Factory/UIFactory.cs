using Infrastructure.Scripts.AssetManagement;
using Source.StaticData;
using Source.StaticData.CharactersCatalog;
using UnityEngine;

namespace Source.UI.Factory
{
    public class UIFactory
    {
        private ResourceLoader _resourceLoader;
        private StaticDataLoadService _staticDataLoadService;

        public UIFactory(ResourceLoader resourceLoader, StaticDataLoadService staticDataLoadService)
        {
            _resourceLoader = resourceLoader;
            _staticDataLoadService = staticDataLoadService;
        }

        public void CreateShop()
        {
            _resourceLoader.Instantiate("Canvases/CharacterSelection_Canvas");
        }

        public void CreateMainMenu()
        {
            CharactersCatalogStaticData config = _staticDataLoadService.LoadCharacterCatalogStaticDatas();
            
            CharacterSkinItem iconPrefab = Resources.Load<CharacterSkinItem>("CharacterSkinItem");

            ShopUI shop = _resourceLoader.Instantiate("Canvases/CharacterSelection_Canvas").GetComponent<ShopUI>();

            foreach (var conf in config.Characters)
            {
                CharacterSkinItem icon = Object.Instantiate(iconPrefab);
                icon.InitImage(conf.Image);
                shop.AddItem(icon);
            }
        }
    }
}