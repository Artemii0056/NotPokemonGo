using Entitas;
using TestECS.Gameplay.Features.Effects;
using TestECS.Gameplay.Features.Effects.Factory;
using TestECS.TimeService;

namespace TestECS.Gameplay.Features.Statuses.Systems
{
    public class PeriodicDamageStatusSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IEffectFactory _effectFactory;
        private readonly IGroup<GameEntity> _statuses;

        public PeriodicDamageStatusSystem(GameContext gameContext, ITimeService timeService, IEffectFactory effectFactory)
        {
            _timeService = timeService;
            _effectFactory = effectFactory;
            _statuses = gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Status,
                    GameMatcher.Period,
                    GameMatcher.TimeSinceLastTick,
                    GameMatcher.EffectValue,
                    GameMatcher.ProducerId,
                    GameMatcher.TargetId));
        }

        public void Execute()
        {
            foreach (GameEntity status in _statuses)
            {
                if (status.TimeSinceLastTick >= 0)
                {
                    status.ReplaceTimeLeft(status.TimeSinceLastTick - _timeService.DeltaTime);
                }
                else
                {
                    status.ReplaceTimeSinceLastTick(status.Period);
                    
                    _effectFactory.CreateEffect(new EffectSetup{ EffectTypeId = EffectTypeId.Damage, Value = status.EffectValue },
                        status.ProducerId,
                        status.TargetId );
                }
            }
        }
    }
}