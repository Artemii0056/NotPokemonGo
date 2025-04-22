using Code.Extensions;
using Entitas;

namespace TestECS.Gameplay.Enemies.Systems
{
    public class EnemyDeathSystem : IExecuteSystem
    {
        private const float DeathAnimationTime = 2;
        private readonly IGroup<GameEntity> _entities;

        public EnemyDeathSystem(GameContext gameContext)
        {
            _entities = gameContext.GetGroup(GameMatcher
                .AllOf(
                GameMatcher.Enemy,
                GameMatcher.Dead,
                GameMatcher.ProcessingDeath));
        }
        
        public void Execute()
        {
            foreach (var enemy in _entities)
            {
                enemy.isMovementAvailable = false;
                
                enemy.RemoveTargetCollectionComponents();

                if (enemy.hasEnemyAnimator) 
                    enemy.EnemyAnimator.PlayDead();

                enemy.ReplaceSelfDestructTimer(DeathAnimationTime);
            }
        }
    }
}