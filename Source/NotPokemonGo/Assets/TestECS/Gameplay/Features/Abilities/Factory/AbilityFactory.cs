using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Features.Abilities.Configs;
using TestECS.Gameplay.Features.Cooldowns;
using TestECS.Gameplay.Hero.Registrars;
using TestECS.Services.StaticData;

namespace TestECS.Gameplay.Features.Abilities.Factory
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly IIdService _idService;
        private readonly IStaticDataService _staticDataService;

        public AbilityFactory(IIdService idService, IStaticDataService staticDataService)
        {
            _idService = idService;
            _staticDataService = staticDataService;
        }

        public GameEntity CreateVegetableBoltAbility(int level)
        {
            AbilityLevel abilityLevel = _staticDataService.GetAbilityLevel(AbilityId.VegetableBolt, level);

            return CreateEntity.Empty()
                .AddId(_idService.Next())
                .AddAbilityId(AbilityId.VegetableBolt)
                .AddCooldown(abilityLevel.Cooldown)
                .With(x => x.isVegetableBoltAbility = true)
                .PutOnCooldown();
        }

        public GameEntity CreateRadialBoltAbility(int level)
        {
            AbilityLevel abilityLevel = _staticDataService.GetAbilityLevel(AbilityId.RadialBolt, level);

            return CreateEntity.Empty()
                .AddId(_idService.Next())
                .AddAbilityId(AbilityId.RadialBolt)
                .AddCooldown(abilityLevel.Cooldown)
                .With(x => x.isRadialBoltAbility = true)
                .PutOnCooldown();
        }
    }
}