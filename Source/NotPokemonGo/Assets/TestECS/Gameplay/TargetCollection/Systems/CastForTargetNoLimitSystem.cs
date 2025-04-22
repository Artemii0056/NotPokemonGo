using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Common.GamePhysics._3D;
using Entitas;

namespace TestECS.Gameplay.TargetCollection.Systems
{
    public class CastForTargetNoLimitSystem : IExecuteSystem
    {
        private readonly IPhysicsService3D _physicsService3D;
        private readonly IGroup<GameEntity> _ready;
        private readonly List<GameEntity> _buffer = new(64);

        public CastForTargetNoLimitSystem(GameContext gameContext, IPhysicsService3D physicsService3D)
        {
            _physicsService3D = physicsService3D;

            _ready = gameContext.GetGroup(GameMatcher.AllOf(
                    GameMatcher.TargetsBuffer,
                    GameMatcher.ReadyToCollectTargets,
                    GameMatcher.WorldPosition,
                    GameMatcher.LayerMask,
                    GameMatcher.Radius)
                .NoneOf(GameMatcher.TargetLimit));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _ready.GetEntities(_buffer))
            {
                entity.TargetsBuffer.AddRange(TargetsInRadius(entity));

                if (!entity.isCollectingTargetsContinuously) 
                    entity.isReadyToCollectTargets = false;
            }
        }

        private IEnumerable<int> TargetsInRadius(GameEntity entity)
        {
            return _physicsService3D
                .SphereCast(entity.WorldPosition, entity.Radius, entity.LayerMask)
                .Select(x => x.Id);
        }
    }
}