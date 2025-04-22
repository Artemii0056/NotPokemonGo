using Entitas;
using TestECS.TimeService;

namespace TestECS.Gameplay.Features.Movement.Systems
{
    public class MoveByDirection : IExecuteSystem
    {
        private readonly ITimeService _time;
        private readonly IGroup<GameEntity> _movers;

        public MoveByDirection(GameContext gameContext, ITimeService time)
        {
            _time = time;
            _movers = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.WorldPosition,
                    GameMatcher.Speed,
                    GameMatcher.MovementAvailable,
                    GameMatcher.Moving,
                    GameMatcher.Direction));
        }

        public void Execute()
        {
            foreach (var mover in _movers) 
                mover.ReplaceWorldPosition(mover.WorldPosition + mover.Direction * mover.Speed * _time.DeltaTime);
        }
    }
}