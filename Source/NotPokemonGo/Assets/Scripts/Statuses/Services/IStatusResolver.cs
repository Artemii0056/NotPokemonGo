using Units;

namespace Statuses.Services
{
    public interface IStatusResolver
    {
        void Resolve(Status status, Unit target);
    }
}