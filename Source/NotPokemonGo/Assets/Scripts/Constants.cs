using UnityEngine;

public static class Constants
{
    public class AssetPath
    {
        public const string InitialSceneName = "Initial";
        public const string GameplaySceneName = "MainMenu";
        public const string CharacterSelectionSceneName = "CharacterSelection";

        public const string CharacterSkinItemName = "Canvases/UnitSkinItemForChoose";
        public const string StartScreenCanvasName = "Canvases/StartScreen_Canvas";
        public const string ChooseMapCanvasName = "Canvases/ChooseMapContainer_Canvas";
        public const string CharacterSelectionCanvasName = "Canvases/CharacterSelectionScreen_Canvas";
        public const string ChooseUnitsCanvasName = "Canvases/ChooseUnitsForBattle_Canvas";
        public const string MainMenuCanvasPath = "Canvases/MainMenu_Canvas";
        public const string CharacteristicItemViewPath = "Canvases/CharacteristicItem";
        public const string LoosePanelPath = "Canvases/LoosePanel_Canvas";
        public const string BattleInfoUIPath = "Canvases/BattleInfo_Canvas";

        public const string CatalogPath = "Catalog/Catalog";
        public const string AbilityConfigPath = "Abilities";

        public const string StatusTypePath = "Statuses/StatusTypesConfig";
        public const string SpawnPositionConfigsPath = "SpawnPositions";
        public const string PlatoonContainersPath = "BattlefieldPrefabs/SpawnPositions";
        public const string CharacterConfigsPath = "Characters";
        public const string LevelConfigsPath = "LevelConfig";
        public const string AbilitiesPanelPath = "Abilities/AbilitiesPanel_Canvas";
        public const string QteConfigsPath = "QTE";
        public const string StatusConfigsPath = "Statuses";
        public const string CombatTextPath = "Canvases/Status/CombatText";
        public const string DodgeView = "Canvases/Dodges/DodgePanel";
        public const string DodgeConfigPath = "Dodges";
        public const string ParticlesByStatusTypesPath = "Statuses/ParticleSystemByStatusTypes";
    }

    public class Positions
    {
        public static Vector3 Platoon1Position = new Vector3(0, 0, 3);
        public static Vector3 Platoon2Position = new Vector3(0, 0, -3);
    }
        
    public static class BaseAnimations
    {
        public static int Idle = Animator.StringToHash(nameof(Idle));
        public static int Death = Animator.StringToHash(nameof(Death));
        public static int Dodge = Animator.StringToHash(nameof(Dodge));
        public static int TakeDamage = Animator.StringToHash(nameof(TakeDamage)); 
        public static int Break = Animator.StringToHash(nameof(Break)); 
    }
}