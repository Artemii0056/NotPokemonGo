using Units;

namespace Statuses.Services
{
    public interface IStatusManager
    {
        void RegisterStatus(Status status);
        void UnregisterStatus(Status status);
        public void UnregisterStatus(Unit target, StatusType type);
        void TickTurn();
        void RemoveInactive();
        void TickUnitTurn();
    }
}