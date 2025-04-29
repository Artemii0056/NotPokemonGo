using System;
using System.Collections.Generic;
using Infrastructure.View;
using TestECS.Gameplay.Features.Effects;
using TestECS.Gameplay.Features.Statuses;

namespace TestECS.Gameplay.Features.Abilities.Configs
{
    [Serializable]
    public class AbilityLevel
    {
        public float Cooldown;
        
        public EntityBehavior ViewPrefab;
        
        public List<EffectSetup> EffectSetups;
        public List<StatusSetup> StatusSetups;
        
        public ProjectileSetup ProjectileSetup;
    }
}