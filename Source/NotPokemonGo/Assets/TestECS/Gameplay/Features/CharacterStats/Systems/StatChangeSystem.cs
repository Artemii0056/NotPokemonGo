using Entitas;

namespace TestECS.Gameplay.Features.CharacterStats.Systems
{
    public class StatChangeSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _statChanger;
        private readonly IGroup<GameEntity> _statOwners;

        public StatChangeSystem(GameContext gameContext)
        {
            _statChanger = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.StatChange,
                    GameMatcher.TargetId,
                    GameMatcher.EffectValue));
            
            _statOwners = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Id,
                    GameMatcher.BaseStats,
                    GameMatcher.StatModifiers));
        }
        
        public void Execute()
        {
            foreach (GameEntity statOwner in _statOwners)
            foreach (Stats stat in statOwner.BaseStats.Keys)
            {
                statOwner.StatModifiers[stat] = 0;
                
                foreach (GameEntity statChanger in _statChanger)
                {
                    if (statChanger.TargetId == statOwner.Id)
                    {
                        statOwner.StatModifiers[stat] += statChanger.EffectValue;
                    }
                }
            }
        }
    }
}