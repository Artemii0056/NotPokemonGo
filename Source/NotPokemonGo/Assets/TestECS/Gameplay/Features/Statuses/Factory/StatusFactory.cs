using System;
using Code.Entity;
using Code.Extensions;
using TestECS.Gameplay.Hero.Registrars;

namespace TestECS.Gameplay.Features.Statuses.Factory
{
    public class StatusFactory : IStatusFactory
    {
        private readonly IIdService _idService;

        public StatusFactory(IIdService idService)
        {
            _idService = idService;
        }

        public GameEntity CreateStatus(StatusSetup setup, int producerId, int targetId)
        {
            GameEntity status;

            switch (setup.StatusTypeId)
            {
                case StatusTypeId.Poison:
                    status = CreatePoisonStatus(setup, producerId, targetId);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            status.With(x => x.AddDuration(setup.Duration), when: setup.Duration > 0);
            status.With(x => x.AddTimeLeft(setup.Duration), when: setup.Duration > 0);
            status.With(x => x.AddPeriod(setup.Period), when: setup.Period > 0);
            status.With(x => x.AddTimeSinceLastTick(0), when: setup.Period > 0);
            
            return status;
        }

        private GameEntity CreatePoisonStatus(StatusSetup setup, int producerId, int targetId)
        {
           return CreateEntity.Empty()
                .AddId(_idService.Next())
                .AddStatusTypeId(StatusTypeId.Poison)
                .AddEffectValue(setup.Value)
                .AddProducerId(producerId)
                .AddTargetId(targetId)
                .With(x => x.isStatus = true)
                .With(x => x.isPoison = true)
               ;
        }
    }
}