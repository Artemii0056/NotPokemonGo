using Infrastructure.Systems;
using TestECS.Gameplay.Features.Abilities.System;
using TestECS.Gameplay.Features.Cooldowns;

namespace TestECS.Gameplay.Features.Abilities
{
    public class AbilityFeature : Feature
    {
        public AbilityFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<CooldownSystem>());
            
            Add(systemFactory.Create<VegetableAbilitySystem>());
            //Add(systemFactory.Create<RadialBoltAbilitySystem>());
            // Add(systemFactory.Create<BouncingProjectileAbilitySystem>());
        }
    }
}