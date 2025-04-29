using System.Collections.Generic;
using Entitas;
using TestECS.Gameplay.Features.Armaments.Factory;
using TestECS.Gameplay.Features.Cooldowns;
using TestECS.Services.StaticData;

namespace TestECS.Gameplay.Features.Abilities.System
{
    public class BouncingProjectileAbilitySystem : IExecuteSystem    
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _heroes;
        private readonly IGroup<GameEntity> _enemies;
        
        private readonly IStaticDataService _staticDataService;
        private readonly IArmamentsFactory _armamentsFactory;
        
        private List<GameEntity> _buffer = new(4);

        public BouncingProjectileAbilitySystem(GameContext gameContext, IStaticDataService staticDataService, IArmamentsFactory armamentsFactory)
        {
            _staticDataService = staticDataService;
            _armamentsFactory = armamentsFactory;
            
            _abilities = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.BouncingBoltAbility,
                        GameMatcher.CooldownUp));
            
            _heroes = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.Hero,
                        GameMatcher.WorldPosition));
             
            _enemies = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.Enemy,
                        GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (GameEntity ability in _abilities.GetEntities(_buffer))
            foreach (GameEntity hero in _heroes)
            {
                if (_enemies.count <= 0)
                    continue;
                
                _armamentsFactory
                    .CreateBouncingBolt(1, hero.WorldPosition);
                
                ability
                    .PutOnCooldown(_staticDataService.GetAbilityLevel(AbilityId.BouncingBolt, 1).Cooldown);
            }
        }
    }
}