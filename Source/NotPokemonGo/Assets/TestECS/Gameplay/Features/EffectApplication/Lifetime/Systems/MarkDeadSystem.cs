using System.Collections.Generic;
using Entitas;

namespace TestECS.Gameplay.Features.EffectApplication.Lifetime.Systems
{
    public class MarkDeadSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(128);

        public MarkDeadSystem(GameContext gameContext)
        {
            _entities = gameContext.GetGroup(GameMatcher.
                AllOf(GameMatcher.CurrentHealthPoint, 
                    GameMatcher.MaxHealthPoint)
                .NoneOf(GameMatcher.Dead));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                if (entity.CurrentHealthPoint <= 0)
                {
                    entity.isDead = true;
                    entity.isProcessingDeath = true;
                }
            }
        }
    }
}