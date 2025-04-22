using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Hero.Systems
{
    public class SetHeroDirectionByInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _heroes;
        private readonly IGroup<GameEntity> _inputs;

        public SetHeroDirectionByInputSystem(GameContext gameContext)
        {
            _heroes = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Hero,
                    GameMatcher.MovementAvailable));
            
            _inputs = gameContext.GetGroup(GameMatcher.Input);
        }

        public void Execute()
        {
            foreach (GameEntity input in _inputs)
            foreach (GameEntity hero in _heroes)
            {
                hero.isMoving = input.hasAxisInput;

                if (input.hasAxisInput)
                {
                    hero.ReplaceDirection(new Vector3(input.AxisInput.normalized.x, 0f, input.AxisInput.normalized.y));
                }
            }
        }
    }
}