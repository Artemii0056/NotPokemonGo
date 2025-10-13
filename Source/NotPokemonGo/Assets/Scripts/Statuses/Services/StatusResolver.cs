using Units;

namespace Statuses.Services
{
    public class StatusResolver : IStatusResolver
    {
        private IStatusManager _statusManager;

        public StatusResolver(IStatusManager statusManager) => 
            _statusManager = statusManager;

        public void Resolve(Status status, Unit target)
        {
            if (HasStatusEffect(out Status statusToResolve, status.Setup.Type, target) == false)
            {
                _statusManager.RegisterStatusEffect(status); 
                target.AddStatus(status);
                return;
            }

            if (status.IsRefreshed == false)
            {
                statusToResolve.IncreaseTickCount(status.TickCount);
                return;
            }
            
            statusToResolve.Refresh(status);
        }

        private bool HasStatusEffect(out Status status, StatusType type, Unit target)
        {
            status = null;

            foreach (var statusOnTarget in target.ImposedStatuses)
            {
                if (statusOnTarget.Setup.Type == type)
                {
                    status = statusOnTarget;
                    return true;
                }
            }

            return false;
        }
    }
}