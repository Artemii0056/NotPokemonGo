using System.Collections.Generic;
using Code.Extensions;
using Entitas;
using TestECS.Gameplay.Features.Abilities.Configs;
using TestECS.Gameplay.Features.Armaments.Factory;
using TestECS.Gameplay.Features.Cooldowns;
using TestECS.Services.StaticData;
using UnityEngine;

namespace TestECS.Gameplay.Features.Abilities.System
{
    public class RadialBoltAbilitySystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _abilities;
        private readonly IGroup<GameEntity> _heroes;

        private readonly IStaticDataService _staticDataService;
        private readonly IArmamentsFactory _armamentsFactory;

        private List<GameEntity> _buffer = new(4);

        public RadialBoltAbilitySystem(GameContext gameContext, IStaticDataService staticDataService, IArmamentsFactory armamentsFactory)
        {
            _staticDataService = staticDataService;
            _armamentsFactory = armamentsFactory;
            
            _abilities = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.RadialBoltAbility,
                        GameMatcher.CooldownUp));
            
            _heroes = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.Hero,
                        GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (GameEntity hero in _heroes)
            foreach (GameEntity ability in _abilities.GetEntities(_buffer))
            {
                _armamentsFactory
                    .CreateRadialBolt(1, hero.WorldPosition)
                    .ReplaceDirection((ability.WorldPosition + hero.WorldPosition).normalized)
                    .With(x => x.isMoving = true);

                ability
                    .PutOnCooldown(_staticDataService.GetAbilityLevel(AbilityId.RadialBolt, 1).Cooldown);
            }
        }

        public void CreateFragmentArmaments(Vector3 heroPosition)
        {
            AbilityLevel abilityLevel = _staticDataService.GetAbilityLevel(AbilityId.RadialBolt, 1);
            
            int countFragments = abilityLevel.ProjectileSetup.FragmentsCount;
            
            float angle = 360f / countFragments;

            for (int i = 0; i < countFragments; i++)
            {
                _armamentsFactory
                    .CreateRadialBolt(1, heroPosition)
                    .ReplaceDirection((ability.WorldPosition + heroPosition).normalized)
                    .With(x => x.isMoving = true);
            }
        }
    }
}