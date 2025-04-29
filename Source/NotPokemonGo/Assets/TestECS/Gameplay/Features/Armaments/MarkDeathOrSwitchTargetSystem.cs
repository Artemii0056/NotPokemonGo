using Entitas;

namespace TestECS.Gameplay.Features.Armaments
{
    public class MarkDeathOrSwitchTargetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _armaments;

        public MarkDeathOrSwitchTargetSystem(GameContext gameContext)
        {
            _armaments = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.BouncingBoltAbility,
                    GameMatcher.ReachedToTarget,
                    GameMatcher.ProcessedTargets,
                    GameMatcher.TargetLimit));
        }

        public void Execute()
        {
            foreach (GameEntity armament in _armaments)
            {
                if (armament.TargetLimit < armament.ProcessedTargets.Count)
                {
                    armament.ReplaceTargetLimit(armament.TargetLimit + 1);
                    armament.isReachedToTarget = false;
                }
                else
                {
                    armament.isDestructed = true;
                }
            }
        }
    }
}