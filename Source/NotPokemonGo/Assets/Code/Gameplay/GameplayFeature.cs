using Code.Common.Destruct;
using Infrastructure.Systems;
using Infrastructure.View.Features;
using TestECS.Gameplay.Enemies;
using TestECS.Gameplay.Features.Abilities;
using TestECS.Gameplay.Features.Armaments;
using TestECS.Gameplay.Features.CharacterStats;
using TestECS.Gameplay.Features.EffectApplication;
using TestECS.Gameplay.Features.EffectApplication.Lifetime.Systems;
using TestECS.Gameplay.Features.Effects;
using TestECS.Gameplay.Features.Movement.Features;
using TestECS.Gameplay.Features.Statuses;
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
            Add(factory.Create<EffectApplicationFeature>());
            
            Add(factory.Create<EffectFeature>());
            Add(factory.Create<StatusFeature>());
            
            Add(factory.Create<StatsFeature>());
            
            Add(factory.Create<ProcessDestructedFeature>());
        }
    }
}