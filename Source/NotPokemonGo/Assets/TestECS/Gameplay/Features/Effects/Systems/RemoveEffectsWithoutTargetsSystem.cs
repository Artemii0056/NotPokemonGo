using Entitas;

namespace TestECS.Gameplay.Features.Effects.Systems
{
    public class RemoveEffectsWithoutTargetsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _effects;

        public RemoveEffectsWithoutTargetsSystem(GameContext gameContext)
        {
            _effects = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Effect,
                    GameMatcher.TargetId));
        }

        public void Execute()
        {
            foreach (GameEntity effect in _effects)
            {
                GameEntity target = effect.Target();

                if (target == null) 
                    effect.Destroy();
            }
        }
    }
}