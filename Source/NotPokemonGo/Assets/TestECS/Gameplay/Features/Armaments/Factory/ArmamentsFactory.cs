using System.Collections.Generic;
using Code.Common.Extensions;
using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Features.Abilities;
using TestECS.Gameplay.Features.Abilities.Configs;
using TestECS.Gameplay.Hero.Registrars;
using TestECS.Services.StaticData;
using UnityEngine;

namespace TestECS.Gameplay.Features.Armaments.Factory
{
    public class ArmamentsFactory : IArmamentsFactory
    {
        private readonly IIdService _idService;
        private IStaticDataService _staticDataService;

        public ArmamentsFactory(IIdService idService, IStaticDataService staticDataService)
        {
            _idService = idService;
            _staticDataService = staticDataService;
        }

        public GameEntity CreateVegetableBolt(int level, Vector3 at)
        {
            AbilityLevel abilityLevel = _staticDataService.GetAbilityLevel(AbilityId.VegetableBolt, level);
            ProjectileSetup setup = abilityLevel.ProjectileSetup;

            return CreateEntity.Empty()
                .AddId(_idService.Next())
                .With(x => x.isArmament = true)
                .AddViewPrefab(abilityLevel.ViewPrefab)
                .AddWorldPosition(at)
                .AddSpeed(setup.Speed)
                .AddDamage(1)
                .AddRadius(setup.ContactRadius)
                .AddTargetsBuffer(new List<int>(16))
                .AddProcessedTargets(new List<int>(16))
                .AddTargetLimit(setup.Pierce)
                .AddLayerMask(CollisionLayer.Enemy.AsMask())
                .With(x => x.isMovementAvailable = true)
                .With(x => x.isReadyToCollectTargets = true)
                .With(x => x.isCollectingTargetsContinuously = true)
                .AddSelfDestructTimer(setup.Lifetime)
                ;
        }

        public GameEntity CreateRadialBolt(int level, Vector3 at)
        {
            AbilityLevel abilityLevel = _staticDataService.GetAbilityLevel(AbilityId.RadialBolt, level);
            ProjectileSetup setup = abilityLevel.ProjectileSetup;

            return CreateEntity.Empty()
                    .AddId(_idService.Next())
                    .With(x => x.isArmament = true)
                    .AddViewPrefab(abilityLevel.ViewPrefab)
                    .AddWorldPosition(at)
                    .AddSpeed(setup.Speed)
                    .AddDamage(1)
                    .AddRadius(setup.ContactRadius)
                    .AddTargetsBuffer(new List<int>(16))
                    .AddProcessedTargets(new List<int>(16))
                    .AddTargetLimit(setup.Pierce)
                    .AddLayerMask(CollisionLayer.Enemy.AsMask())
                    .With(x => x.isMovementAvailable = true)
                    .With(x => x.isReadyToCollectTargets = true)
                    .With(x => x.isCollectingTargetsContinuously = true)
                    .AddSelfDestructTimer(setup.Lifetime)
                ;
        }
    }
}