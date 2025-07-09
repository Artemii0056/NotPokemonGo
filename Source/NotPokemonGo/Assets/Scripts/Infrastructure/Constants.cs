using UnityEngine;

namespace Infrastructure
{
    public static class Constants
    {
        public class AssetPath
        {
            public const string InitialSceneName = "Initial";
            public const string MainMenuSceneName = "MainMenu";
            public const string CharacterSelectionSceneName = "CharacterSelection";

            public const string CharacterSkinItemName = "Canvases/CharacterSkinItem";
            public const string StartScreenCanvasName = "Canvases/StartScreen_Canvas";
            public const string ChooseMapCanvasName = "Canvases/ChooseMapContainer_Canvas";
            public const string CharacterSelectionCanvasName = "Canvases/CharacterSelectionScreen_Canvas";
            public const string MainMenuCanvasPath = "Canvases/MainMenu_Canvas";
            public const string CharacteristicItemViewPath = "Canvases/CharacteristicItem";

            public const string CatalogPath = "Catalog/Catalog";
            public const string AbilityConfigPath = "Abilities";

            public const string StatusTypePath = "Statuses/StatusTypesConfig";
            public const string SpawnPositionConfigsPath = "SpawnPositions";
            public const string CharacterConfigsPath = "LevelConfig";
            public const string AbilitiesPanelPath = "Abilities/AbilitiesPanel_Canvas";
        }

        public class Positions
        {
            public static Vector3 Platoon1Position = new Vector3(0, 0, 3);
            public static Vector3 Platoon2Position = new Vector3(0, 0, -3);
        }
        
        public static class BaseAnimations
        {
            public static int Idle = Animator.StringToHash(nameof(Idle));
        }
    }
}