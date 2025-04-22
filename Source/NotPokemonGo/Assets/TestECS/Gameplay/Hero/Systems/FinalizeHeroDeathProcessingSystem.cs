using System.Collections.Generic;
using Entitas;

namespace TestECS.Gameplay.Hero.Systems
{
    public class FinalizeHeroDeathProcessingSystem : IExecuteSystem
    {
        private IGroup<GameEntity> _heroes;
        private List<GameEntity> _buffer = new(1);

        public FinalizeHeroDeathProcessingSystem(GameContext gameContext)
        {
            _heroes = gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Hero,
                    GameMatcher.Dead,
                    GameMatcher.ProcessingDeath));
        }
        
        public void Execute()
        {
            foreach (GameEntity hero in _heroes.GetEntities(_buffer)) 
                hero.isProcessingDeath = true;
        }
    }
}