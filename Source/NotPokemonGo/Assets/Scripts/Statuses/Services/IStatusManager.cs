namespace Statuses.Services
{
    public interface IStatusManager
    {
        void RegisterStatus(Status status);
        void UnregisterStatus(Status status);
        void TickTurn();
        void RemoveInactive();
        void TickUnitTurn();
    }
}