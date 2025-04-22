using System.Collections.Generic;
using Entitas;

namespace TestECS.Gameplay.Enemies.Systems
{
    public class FinalizeEnemyDeathProcessingSystem : IExecuteSystem
    {
        private readonly GameContext _gameContext;
        private IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(128);

        public FinalizeEnemyDeathProcessingSystem(GameContext gameContext)
        {
            _gameContext = gameContext;
            
            _entities = _gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Enemy,
                    GameMatcher.Dead,
                    GameMatcher.ProcessingDeath));
        }
        
        public void Execute()
        {
            foreach (GameEntity enemy in _entities.GetEntities(_buffer)) 
                enemy.isProcessingDeath = true;
        }
    }
}