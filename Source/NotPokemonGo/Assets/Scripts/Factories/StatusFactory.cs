using System;
using Effects;
using Statuses;
using Units;

namespace Factories
{
    public class StatusFactory : IStatusFactory
    {
        public Status Create(StatusSetup setup, Unit target, IEffectResolver effectResolver)
        {
            Status status = null;

            switch (setup.Type)
            {
                case StatusType.Damage:
                    status = new DamageStatus(setup, target, effectResolver);
                    break;
                
                case StatusType.Heal:
                    status = new HealStatus(setup, target, effectResolver);
                    break;
                
                case StatusType.Poison:
                    break;
                
                case StatusType.PositiveSpeed:
                    break;
                
                case StatusType.Stun:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return status;
        }
    }
}