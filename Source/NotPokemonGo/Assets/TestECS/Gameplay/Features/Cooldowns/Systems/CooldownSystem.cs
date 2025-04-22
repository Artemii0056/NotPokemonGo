using System.Collections.Generic;
using Entitas;
using TestECS.TimeService;

namespace TestECS.Gameplay.Features.Cooldowns
{
    public class CooldownSystem : IExecuteSystem
    {
        private readonly ITimeService _timeService;
        private readonly IGroup<GameEntity> _cooldowns;
        private List<GameEntity> _buffer = new(64);

        public CooldownSystem(GameContext gameContext, ITimeService timeService)
        {
            _timeService = timeService;
            _cooldowns = gameContext.GetGroup(GameMatcher
                .AllOf(GameMatcher.Cooldown,
                    GameMatcher.CooldownLeft));
        }

        public void Execute()
        {
            foreach (GameEntity cooldown in _cooldowns.GetEntities(_buffer))
            {
                cooldown.ReplaceCooldownLeft(cooldown.CooldownLeft - _timeService.DeltaTime);

                if (cooldown.CooldownLeft <= 0)
                {
                    cooldown.isCooldownUp = true;
                    cooldown.RemoveCooldownLeft();
                }
            }
        }
    }
}