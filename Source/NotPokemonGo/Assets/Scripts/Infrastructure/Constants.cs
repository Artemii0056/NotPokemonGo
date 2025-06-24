using UnityEngine;

namespace Infrastructure
{
    public static class Constants
    {
        public class AssetPath
        {
            public const string InitialSceneName = "Initial";
            public const string MainMenuSceneName = "MainMenu";

            public const string CharacterSkinItemName = "Canvases/CharacterSkinItem";
            public const string CharacterSelectionCanvasName = "Canvases/CharacterSelectionScreen_Canvas";
            public const string MainMenuCanvasPath = "Canvases/MainMenu_Canvas";
            public const string CharacteristicItemViewPath = "Canvases/CharacteristicItem";

            public const string CatalogPath = "Catalog/Catalog";
            public const string AbilityConfigPath = "Abilities";

            public const string StatusTypePath = "Statuses/StatusTypesConfig";
            public const string SpawnPositionConfigsPath = "SpawnPositions";
            public const string CharacterConfigsPath = "Characters";
            public const string AbilitiesPanelPath = "Abilities/AbilitiesPanel_Canvas";
        }

        public class Positions
        {
            public static Vector3 Platoon1Position = new Vector3(0, 0, 3);
            public static Vector3 Platoon2Position = new Vector3(0, 0, -3);
        }

        public class AnimationsName
        {
            public const string Idle = nameof(Idle);
            public const string Death = nameof(Death);
            public const string Dodge = nameof(Dodge);
            public const string TakeDamage = nameof(TakeDamage);

            public class Mage
            {
                public const string FireballAttack = nameof(FireballAttack);
                public const string CastSpell = nameof(CastSpell);
                public const string RadialAttack = nameof(RadialAttack);
            }

            public class Swordsman
            {
                public const string TwoSwordsAttack = nameof(TwoSwordsAttack);
            }

            public class Archer
            {
                public const string MiddleShoot = nameof(MiddleShoot);
                public const string ShootInSky = nameof(ShootInSky);

            }
        }
    }
}