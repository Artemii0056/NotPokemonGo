using Entitas;

namespace TestECS.Gameplay.Hero.Systems
{
    public class HeroDeathSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _heroes;

        public HeroDeathSystem(GameContext gameContext)
        {
            _heroes = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Hero, 
                    GameMatcher.Dead,
                    GameMatcher.HeroAnimator,
                    GameMatcher.ProcessingDeath));
        }
        
        public void Execute()
        {
            foreach (GameEntity hero in _heroes)
            {
                hero.isMovementAvailable = false;

                hero.HeroAnimator.PlayDied();
            }
        }
    }
}