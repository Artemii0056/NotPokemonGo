using System.Collections.Generic;
using Entitas;
using TestECS.Gameplay.Features.Effects;

namespace TestECS.Gameplay.Features.Statuses.Systems.StatusVisualsFeature.Visuals
{
    public class ApplyPoisonVisualsSystem : ReactiveSystem<GameEntity>
    {
        public ApplyPoisonVisualsSystem(GameContext gameContext) : base(gameContext)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) => 
            context.CreateCollector(GameMatcher.Poison.Added());

        protected override bool Filter(GameEntity entity) => 
            entity.isStatus && entity.isPoison && entity.hasTargetId;

        protected override void Execute(List<GameEntity> statuses)
        {
            foreach (GameEntity status in statuses)
            {
                GameEntity target = status.Target();
                
                if (target is {hasStatusVisuals: true}) 
                    target.StatusVisuals.ApplyPoison();
            }
        }
    }
}