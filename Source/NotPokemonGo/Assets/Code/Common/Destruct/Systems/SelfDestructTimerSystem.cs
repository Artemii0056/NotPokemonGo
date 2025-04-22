using System.Collections.Generic;
using Entitas;
using TestECS.TimeService;

namespace Code.Common.Destruct.Systems
{
    public class SelfDestructTimerSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(64);

        public SelfDestructTimerSystem(GameContext gameContext, ITimeService timeService)
        {
            _timeService = timeService;
            _entities = gameContext.GetGroup(GameMatcher.SelfDestructTimer);
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                if (entity.SelfDestructTimer > 0)
                {
                    entity.ReplaceSelfDestructTimer(entity.SelfDestructTimer - _timeService.DeltaTime);
                }
                else
                {
                    entity.RemoveSelfDestructTimer();
                    entity.isDestructed = true;
                }
            }
        }
    }
}