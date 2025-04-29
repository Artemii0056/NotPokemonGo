using Entitas;

namespace TestECS.Gameplay.Features.EffectApplication.Lifetime.Systems
{
    public class UnapplyStatusesOfDeadTargetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statuses;
        private readonly IGroup<GameEntity> _dead;

        public UnapplyStatusesOfDeadTargetSystem(GameContext gameContext)
        {
            _statuses = gameContext.GetGroup(GameMatcher.
                AllOf(GameMatcher.Status, 
                    GameMatcher.TargetId));
            
            _dead = gameContext.GetGroup(GameMatcher.
                AllOf(GameMatcher.Id, 
                    GameMatcher.Dead));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _dead)
            foreach (GameEntity status in _statuses)
            {
                if (status.TargetId == entity.Id) 
                    status.isUnapplied = true;
            }
        }
    }
}