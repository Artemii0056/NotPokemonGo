using System;
using Infrastructure.View;

namespace TestECS.Gameplay.Features.Abilities.Configs
{
    [Serializable]
    public class AbilityLevel
    {
        public float Cooldown;
        
        public EntityBehavior ViewPrefab;
        
        public ProjectileSetup ProjectileSetup;
    }
}