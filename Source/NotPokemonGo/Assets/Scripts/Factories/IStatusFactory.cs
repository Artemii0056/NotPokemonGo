using Effects;
using Statuses;
using Units;

namespace Factories
{
    public interface IStatusFactory
    {
        Status Create(StatusSetup setup, Unit target, IEffectResolver effectResolver);
    }
}