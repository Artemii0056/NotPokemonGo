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
            public static int Idle =  Animator.StringToHash(nameof(Idle));
            public static int Death =  Animator.StringToHash(nameof(Death));
            public static int Dodge =  Animator.StringToHash(nameof(Dodge));
            public static int TakeDamage = Animator.StringToHash( nameof(TakeDamage));

            public class Mage
            {
                public static int FireballAttack = Animator.StringToHash( nameof(FireballAttack));
                public static int CastSpell =  Animator.StringToHash(nameof(CastSpell));
                public static int RadialAttack =  Animator.StringToHash(nameof(RadialAttack));
            }

            public class Swordsman
            {
                public static int TwoSwordsAttack =  Animator.StringToHash(nameof(TwoSwordsAttack));
            }

            public class Archer
            {
                public static int MiddleShoot =  Animator.StringToHash(nameof(MiddleShoot));
                public static int ShootInSky =  Animator.StringToHash(nameof(ShootInSky));

            }
        }
    }
}