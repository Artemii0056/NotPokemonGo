using System.Linq;
using Code.Extensions;
using TestECS.Gameplay.Code.Common.EntityIndices;
using TestECS.Gameplay.Features.Statuses;
using TestECS.Gameplay.Features.Statuses.Factory;

namespace TestECS.Gameplay.Features.Applier
{
    public class StatusApplier : IStatusApplier
    {
        private readonly IStatusFactory _statusFactory;
        private readonly GameContext _gameContext;

        public StatusApplier(IStatusFactory statusFactory, GameContext gameContext)
        {
            _statusFactory = statusFactory;
            _gameContext = gameContext;
        }

        public GameEntity ApplyStatus(StatusSetup statusSetup, int producerId, int targetId)
        {
            GameEntity statusEntity = _gameContext.TargetStatusesOfType(statusSetup.StatusTypeId, targetId)
                .FirstOrDefault();

            if (statusEntity != null)
                return statusEntity.ReplaceTimeLeft(statusSetup.Duration);
            else
                return _statusFactory.CreateStatus(statusSetup, producerId, targetId)
                    .With(x => x.isApplied = true);
        }
    }
}