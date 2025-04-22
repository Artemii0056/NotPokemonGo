using Code.Common.Destruct;
using Infrastructure.Systems;
using Infrastructure.View.Features;
using TestECS.Gameplay.Enemies;
using TestECS.Gameplay.Features.Abilities;
using TestECS.Gameplay.Features.Armaments;
using TestECS.Gameplay.Features.DamageApplication;
using TestECS.Gameplay.Features.DamageApplication.Lifetime.Systems;
using TestECS.Gameplay.Features.Movement.Features;
using TestECS.Gameplay.Hero;
using TestECS.Gameplay.Input;
using TestECS.Gameplay.TargetCollection.Features;

namespace Code.Gameplay
{
    public class GameplayFeature : Feature
    {
        public GameplayFeature(ISystemFactory factory)
        {
            Add(factory.Create<InputFeature>());
            Add(factory.Create<BindViewFeature>());
            
            Add(factory.Create<HeroFeature>());
            Add(factory.Create<EnemyFeature>());
            Add(factory.Create<DeathFeature>());
            
            Add(factory.Create<MovementFeature>());
            Add(factory.Create<AbilityFeature>());
            
            Add(factory.Create<ArmamentFeature>());
            
            Add(factory.Create<CollectTargetFeature>());
            Add(factory.Create<DamageApplicationFeature>());
            
            Add(factory.Create<ProcessDestructedFeature>());
        }
    }
}