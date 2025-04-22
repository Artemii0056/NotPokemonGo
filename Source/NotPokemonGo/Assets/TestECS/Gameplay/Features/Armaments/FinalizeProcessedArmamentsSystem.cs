using Code.Extensions;
using Entitas;

namespace TestECS.Gameplay.Features.Armaments
{
    public class FinalizeProcessedArmamentsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _armaments;

        public FinalizeProcessedArmamentsSystem(GameContext gameContext)
        {
            _armaments = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.Processed));
        }
        
        public void Execute()
        {
            foreach (GameEntity armament in _armaments)
            {
                armament.RemoveTargetCollectionComponents();
                armament.isDestructed = true;
            }
        }
    }
}