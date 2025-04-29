using Entitas;
using TestECS.Gameplay.Features.Effects;
using TestECS.Gameplay.Features.Effects.Factory;

namespace TestECS.Gameplay.Features.Statuses
{
    public class ApplyStatusesOnTargetsSystem : IExecuteSystem
    {
        private readonly IEffectFactory _effectFactory;
        private readonly IGroup<GameEntity> _entities;

        public ApplyStatusesOnTargetsSystem(GameContext gameContext, IEffectFactory effectFactory)
        {
            _effectFactory = effectFactory;
            
            _entities = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.TargetsBuffer,
                    GameMatcher.EffectSetups));
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            foreach (int targetId in entity.TargetsBuffer)
            foreach (EffectSetup setup in entity.EffectSetups)
            {
                _effectFactory.CreateEffect(setup, ProducerId(entity), targetId);
            }
        }

        private static int ProducerId(GameEntity entity)
        {
            return entity.hasProducerId ? entity.ProducerId : entity.Id;
        }
    }
}