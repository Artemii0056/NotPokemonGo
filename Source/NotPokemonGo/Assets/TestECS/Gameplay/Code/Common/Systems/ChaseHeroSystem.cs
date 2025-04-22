using Entitas;

namespace TestECS.Gameplay.Code.Common.Systems
{
    public class ChaseHeroSystem : IExecuteSystem
    {
        private readonly GameContext _gameContext;

        private readonly IGroup<GameEntity> _movers;
        private readonly IGroup<GameEntity> _players;

        public ChaseHeroSystem(GameContext gameContext)
        {
            _gameContext = gameContext;
            _movers = gameContext.GetGroup(GameMatcher.AllOf(
                GameMatcher.WorldPosition,
                GameMatcher.Speed,
                GameMatcher.Enemy));

            _players = _gameContext.GetGroup(GameMatcher.AllOf(
                GameMatcher.Hero,
                GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (GameEntity player in _players)
            foreach (GameEntity mover in _movers)
            {
                mover.ReplaceDirection((player.WorldPosition - mover.WorldPosition).normalized);
                mover.isMoving = true;
            }
        }
    }
}