using System.Collections.Generic;
using System.Linq;
using Code.Extensions;
using Entitas;
using NUnit.Framework.Constraints;
using TestECS.Gameplay.Features.Armaments.Factory;
using TestECS.Gameplay.Features.Cooldowns;
using TestECS.Services.StaticData;
using Unity.Android.Gradle.Manifest;
using UnityEditorInternal;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace TestECS.Gameplay.Features.Abilities.System
{
    public class VegetableAbilitySystem : IExecuteSystem    
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _heroes;
        private readonly IGroup<GameEntity> _enemies;
        
        private readonly IStaticDataService _staticDataService;
        private readonly IArmamentsFactory _armamentsFactory;
        
        private List<GameEntity> _buffer = new(4);

        public VegetableAbilitySystem(GameContext gameContext, IStaticDataService staticDataService, IArmamentsFactory armamentsFactory)
        {
            _staticDataService = staticDataService;
            _armamentsFactory = armamentsFactory;
            
            _abilities = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.VegetableBoltAbility,
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
                    .CreateVegetableBolt(1, hero.WorldPosition)
                    .ReplaceDirection((FirstAvailableTarget().WorldPosition - hero.WorldPosition).normalized)
                    .With(x => x.isMoving = true);
                
                ability
                    .PutOnCooldown(_staticDataService.GetAbilityLevel(AbilityId.VegetableBolt, 1).Cooldown);
            }
        }

        private GameEntity FirstAvailableTarget()
        {
            return _enemies.AsEnumerable().First();
        }
    }
}