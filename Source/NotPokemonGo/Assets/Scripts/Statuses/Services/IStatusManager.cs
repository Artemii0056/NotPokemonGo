namespace Statuses.Services
{
    public interface IStatusManager
    {
        void RegisterStatusEffect(Status status);
        void UnregisterStatusEffect(Status status);
        void Tick();
        void RemoveInactive();
    }
}