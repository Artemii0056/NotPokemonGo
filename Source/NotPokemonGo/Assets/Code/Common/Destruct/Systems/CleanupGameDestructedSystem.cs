using System.Collections.Generic;
using Entitas;

namespace Code.Common.Destruct.Systems
{
    public class CleanupGameDestructedSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(64);

        public CleanupGameDestructedSystem(GameContext gameContext) =>
            _entities = gameContext.GetGroup(GameMatcher.Destructed);

        public void Cleanup()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
                entity.Destroy();
        }
    }
}