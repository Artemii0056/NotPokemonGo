using Infrastructure.Systems;
using TestECS.Gameplay.Hero.Systems;

namespace TestECS.Gameplay.Hero
{
    public class HeroFeature : Feature
    {
        public HeroFeature(ISystemFactory factory)
        {
            Add(factory.Create<InitializeHeroSystem>());
            
            Add(factory.Create<SetHeroDirectionByInputSystem>());
            Add(factory.Create<AnimateHeroMovementSystem>());
            Add(factory.Create<HeroDeathSystem>());
            
            Add(factory.Create<FinalizeHeroDeathProcessingSystem>());
        }
    }
}