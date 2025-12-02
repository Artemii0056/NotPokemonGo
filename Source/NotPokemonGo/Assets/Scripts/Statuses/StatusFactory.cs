using System;
using Effects;
using Units;

namespace Statuses
{
    public class StatusFactory : IStatusFactory
    {
        public Status Create(StatusSetup setup, Unit source, Unit target, IEffectResolver effectResolver)
        {
            Status status = null;

            switch (setup.Type)
            {
                case StatusType.Damage:
                    status = new DamageStatus(setup, source, target, effectResolver);
                    break;

                case StatusType.Heal:
                    status = new HealStatus(setup, source, target, effectResolver);
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