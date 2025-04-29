using Entitas;
using TestECS.Gameplay.Features.Abilities.Factory;
using TestECS.Gameplay.Hero.Factory;
using TestECS.Levels;

namespace TestECS.Gameplay.Hero.Systems
{
    public class InitializeHeroSystem : IInitializeSystem
    {
        private readonly IHeroFactory _heroFactory;
        private readonly ILevelDataProvider _levelDataProvider;
        private readonly IAbilityFactory _abilityFactory;

        public InitializeHeroSystem(IHeroFactory heroFactory, ILevelDataProvider levelDataProvider, IAbilityFactory abilityFactory)
        {
            _heroFactory = heroFactory;
            _levelDataProvider = levelDataProvider;
            _abilityFactory = abilityFactory;
        }

        public void Initialize()
        {
            _heroFactory.Create(_levelDataProvider.StartPoint);
            _abilityFactory.CreateVegetableBoltAbility(1);
            _abilityFactory.CreateRadialBoltAbility(1);
            _abilityFactory.CreateBouncingBoltAbility(1);
        }
    }
}