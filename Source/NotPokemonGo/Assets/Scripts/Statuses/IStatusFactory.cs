using Units;

namespace Statuses
{
    public interface IStatusFactory
    {
        Status Create(StatusSetup setup, Unit target);
    }
}