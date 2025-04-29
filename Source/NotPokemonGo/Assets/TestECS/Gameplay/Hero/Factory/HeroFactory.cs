using Code.Common;
using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Features.CharacterStats;
using TestECS.Gameplay.Hero.Registrars;
using UnityEngine;

namespace TestECS.Gameplay.Hero.Factory
{
    public class HeroFactory : IHeroFactory
    {

        private readonly IIdService _idService;

        public HeroFactory(IIdService idService)
        {
            _idService = idService;
        }

        public GameEntity Create(Vector3 at)
        {
            var baseStats = InitStats.EmptyStatDictionary()
                    .With(x => x[Stats.Speed] = 4)
                    .With(x => x[Stats.MaxHealth] = 100)
                ;

            return CreateEntity.Empty()
                .AddId(_idService.Next())
                .AddWorldPosition(at)
                .AddDirection(Vector2.zero)
                .AddBaseStats(baseStats)
                .AddStatModifiers(InitStats.EmptyStatDictionary())
                .AddSpeed(baseStats[Stats.Speed])
                .AddCurrentHealthPoint(baseStats[Stats.MaxHealth])
                .AddMaxHealthPoint(baseStats[Stats.MaxHealth])
                .AddViewPath("Hero/Prefab/Hero")
                .With(x => x.isHero = true)
                .With(x => x.isMovementAvailable = true);
        }
    }
}