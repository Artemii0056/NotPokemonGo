using Effects;
using Units;

namespace Statuses
{
    public interface IStatusFactory
    {
        Status Create(StatusSetup setup, Unit source, Unit target, IEffectResolver effectResolver);
    }
}