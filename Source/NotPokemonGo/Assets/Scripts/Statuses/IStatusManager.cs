namespace Statuses
{
    public interface IStatusManager
    {
        void RegisterStatusEffect(Status status);
        void UnregisterStatusEffect(Status status);
        void Update(float deltaTime);
        void RemoveInactive();
    }
}