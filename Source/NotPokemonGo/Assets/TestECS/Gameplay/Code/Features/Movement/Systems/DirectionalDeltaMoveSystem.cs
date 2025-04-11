using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Code.Features.Movement.Systems
{
    public class DirectionalDeltaMoveSystem : IExecuteSystem
    {
        private readonly ITimeService _time;
        private readonly IGroup<GameEntity> _movers;
        
        public DirectionalDeltaMoveSystem(GameContext gameContext, ITimeService time)
        {
            _time = time;
            _movers = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.WorldPosition,
                    GameMatcher.Speed,
                    GameMatcher.Moving,
                    GameMatcher.Direction));
        }

        public void Execute()
        {
            foreach (var mover in _movers)
            {
                mover.ReplaceWorldPosition((Vector2)mover.WorldPosition + mover.Direction * mover.Speed * _time.DeltaTime);
            }
        }
    }
}