using System.Collections.Generic;
using Entitas;
using TestECS.Gameplay.Features.Statuses;
using TestECS.Gameplay.Features.Statuses.Indexing;
using TestECS.Gameplay.TargetCollection;
using Zenject;

namespace TestECS.Gameplay.Code.Common.EntityIndices
{
    
    
    public class GameEntityIndices : IInitializable
    {
        private readonly GameContext _gameContext;
        
        public const string StatusesOfType = "StatusesOfType";

        public GameEntityIndices(GameContext gameContext)
        {
            _gameContext = gameContext;
        }
        
        public void Initialize()
        {
            _gameContext.AddEntityIndex(new EntityIndex<GameEntity,StatusKey>(
                name: StatusesOfType,
                _gameContext.GetGroup(GameMatcher.AllOf(
                        GameMatcher.StatusTypeId,
                        GameMatcher.TargetId,
                        GameMatcher.Status,
                        GameMatcher.Duration,
                        GameMatcher.TimeLeft)),
                getKey: GetTargetStatusKey,
                new StatusKeyEqualityComparer()));
        }

        private StatusKey GetTargetStatusKey(GameEntity entity, IComponent component)
        {
            return new StatusKey(
              (component as TargetCollectionComponents.TargetId)?.Value ?? entity.TargetId,
              (component as StatusComponents.StatusTypeIdComponent)?.Value ?? entity.StatusTypeId);
        }
    }

    public static class ContextIndicesExtensions
    {
        public static HashSet<GameEntity> TargetStatusesOfType(this GameContext gameContext,
            StatusTypeId statusTypeId, int targetId)
        {
            return ((EntityIndex<GameEntity, StatusKey>)gameContext.GetEntityIndex(GameEntityIndices.StatusesOfType))
                .GetEntities(new StatusKey(targetId, statusTypeId));
        }
    }
}