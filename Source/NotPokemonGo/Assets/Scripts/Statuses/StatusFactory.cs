using Effects;
using Units;

namespace Statuses
{
    public class StatusFactory : IStatusFactory
    {
        public Status Create(StatusSetup setup, Unit source, Unit target, IEffectResolver effectResolver) => 
            new( setup,  source,  target,  effectResolver);
    }
}