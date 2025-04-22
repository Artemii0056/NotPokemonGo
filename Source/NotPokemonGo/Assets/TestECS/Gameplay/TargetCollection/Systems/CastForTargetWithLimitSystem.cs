using System;
using System.Collections.Generic;
using Code.Gameplay.Common.GamePhysics._3D;
using Entitas;

namespace TestECS.Gameplay.TargetCollection.Systems
{
    public class CastForTargetWithLimitSystem : IExecuteSystem, ITearDownSystem
    {
        private readonly IPhysicsService3D _physicsService3D;
        private readonly IGroup<GameEntity> _ready;
        private readonly List<GameEntity> _buffer = new(64);
        private GameEntity[] _targetCastBuffer = new GameEntity[128];

        public CastForTargetWithLimitSystem(GameContext gameContext, IPhysicsService3D physicsService3D)
        {
            _physicsService3D = physicsService3D;
            
            _ready = gameContext.GetGroup(GameMatcher.AllOf(
                GameMatcher.ReadyToCollectTargets,
                GameMatcher.TargetsBuffer,
                GameMatcher.WorldPosition,
                GameMatcher.ProcessedTargets,
                GameMatcher.TargetLimit,
                GameMatcher.LayerMask,
                GameMatcher.Radius));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _ready.GetEntities(_buffer))
            {
                for (int i = 0; i < Math.Min(TargetCountInRadius(entity), entity.TargetLimit); i++)
                {
                    var targetId = _targetCastBuffer[i].Id;
                    
                    if (AlreadyProcessed(entity, targetId) == false)
                    {
                        entity.TargetsBuffer.Add(targetId);
                        entity.ProcessedTargets.Add(targetId);
                    }
                }

                if (!entity.isCollectingTargetsContinuously) 
                    entity.isReadyToCollectTargets = false;
            }
        }

        private bool AlreadyProcessed(GameEntity entity, int targetId)
        {
             return entity.ProcessedTargets.Contains(targetId);
        }

        private int TargetCountInRadius(GameEntity entity)
        {
            return _physicsService3D
                .SphereCastNonAlloc(entity.WorldPosition,
                    entity.Radius,
                    entity.LayerMask,
                    _targetCastBuffer);
        }

        public void TearDown()
        {
            _targetCastBuffer = null;
        }
    }
}