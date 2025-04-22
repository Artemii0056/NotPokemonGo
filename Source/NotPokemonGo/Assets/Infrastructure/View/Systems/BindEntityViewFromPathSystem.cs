using System.Collections.Generic;
using Entitas;
using Infrastructure.View.Systems.Factory;

namespace Infrastructure.View.Systems
{
    public class BindEntityViewFromPathSystem : IExecuteSystem
    {
        private readonly IEntityViewFactory _entityViewFactory;
        private readonly IGroup<GameEntity> _entities;
        private readonly List<GameEntity> _buffer = new(32);

        public BindEntityViewFromPathSystem(GameContext context, IEntityViewFactory entityViewFactory)
        {
            _entityViewFactory = entityViewFactory;
            _entities = context.GetGroup(GameMatcher
                .AllOf(GameMatcher.ViewPath)
                .NoneOf(GameMatcher.View));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                _entityViewFactory.CreateViewForEntity(entity);
            }
        }
    }
}