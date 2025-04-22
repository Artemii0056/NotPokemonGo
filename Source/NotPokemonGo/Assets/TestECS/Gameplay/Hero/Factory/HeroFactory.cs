using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Hero.Registrars;
using UnityEngine;

namespace TestECS.Gameplay.Hero.Factory
{
    public class HeroFactory : IHeroFactory
    {
        public int Speed = 2;
        public float MaxHealthPoint = 100;
        
        private readonly IIdService _idService;

        public HeroFactory(IIdService idService)
        {
            _idService = idService;
        }

        public GameEntity Create(Vector3 at)
        {
            return CreateEntity.Empty()
                .AddId(_idService.Next())
                .AddSpeed(Speed)
                .AddWorldPosition(at)
                .AddDirection(Vector2.zero)
                .AddCurrentHealthPoint(MaxHealthPoint)
                .AddMaxHealthPoint(MaxHealthPoint)
                .AddViewPath("Hero/Prefab/Hero")
                .With(x => x.isHero = true)
                .With(x => x.isMovementAvailable = true);
        }
    }
}