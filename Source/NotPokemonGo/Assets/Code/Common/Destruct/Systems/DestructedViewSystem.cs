using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Common.Destruct.Systems
{
    public class DestructedViewSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new List<GameEntity>(64);

        public DestructedViewSystem(GameContext gameContext) =>
            _entities = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.View,
                    GameMatcher.Destructed));

        public void Cleanup()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                entity.View.ReleaseEntity();
                Object.Destroy(entity.View.gameObject);
            }
        }
    }
}