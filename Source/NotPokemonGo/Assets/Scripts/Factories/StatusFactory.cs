using System;
using Effects;
using Statuses;
using Units;

namespace Factories
{
    public class StatusFactory
    {
        public Status Create(StatusSetup setup, Unit target, EffectResolver effectResolver)
        {
            Status status = null;
            
            switch (setup.EffectSetup.Type)
            {
                case EffectType.Damage:
                    break;
                
                case EffectType.Heal:
                    status = new HealStatus(setup, target, effectResolver);
                    break;
                
                case EffectType.Poison:
                    break;
                
                case EffectType.PositiveSpeed:
                    break;
                
                case EffectType.Stun:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(setup.EffectSetup.Type), setup.EffectSetup.Type, null);
            }

            return status;
        }
    }
}