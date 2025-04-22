using Entitas;

namespace TestECS.Gameplay.Features.Movement.Systems
{
    public class UpdateTransformPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _transforms;

        public UpdateTransformPositionSystem(GameContext gameContext)
        {
            _transforms = gameContext.GetGroup(
                GameMatcher.AllOf(
                    GameMatcher.Transform,
                    GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (var mover in _transforms)
            {
                mover.Transform.position = mover.WorldPosition;
            }
        }
    }
}