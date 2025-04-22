using System;
using System.Collections.Generic;
using Code.Common.Extensions;
using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Hero.Registrars;
using UnityEngine;
using Zenject;

namespace TestECS.Gameplay.Enemies.Factory
{
    public class EnemyFactory : IEnemyFactory
    {
        public int Speed = 2;
        public float MaxHealthPoint = 5;
        public float Damage = 1;

        private readonly IIdService _identifier;

        [Inject]
        public EnemyFactory(IIdService identifier)
        {
            _identifier = identifier;
        }

        public GameEntity Create(EnemyTypeId typeId, Vector3 at)
        {
            switch (typeId)
            {
                case EnemyTypeId.First:
                    return CreateFirstType(at);
            }

            throw new Exception($"Does not exist for type {typeId}");
        }

        private GameEntity CreateFirstType(Vector3 at)
        {
            return CreateEntity.Empty()
                .AddId(_identifier.Next())
                .AddEnemyTypeId(EnemyTypeId.First)
                .AddWorldPosition(at)
                .AddSpeed(Speed)
                .AddMaxHealthPoint(MaxHealthPoint)
                .AddCurrentHealthPoint(MaxHealthPoint)
                .AddDamage(Damage)
                .AddTargetsBuffer(new List<int>(1))
                .AddDirection(Vector3.forward)
                .AddRadius(0.3f)
                .AddCollectTargetsInterval(0.5f)
                .AddCollectTargetsTimer(0)
                .AddLayerMask(CollisionLayer.Hero.AsMask())
                .AddViewPath("Enemy/Enemy")
                .With(x => x.isMoving = true)
                .With(x => x.isEnemy = true)
                .With(x => x.isMovementAvailable = true);
        }
    }
}