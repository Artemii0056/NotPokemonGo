using System.Collections.Generic;

namespace Statuses.Services
{
    public class StatusManager : IStatusManager
    {
       private List<Status> _statusEffects = new List<Status>();

        public void RegisterStatusEffect(Status status)
        {
            _statusEffects.Add(status);
            status.OnApply();
        }

        public void UnregisterStatusEffect(Status status)
        {
            status.Target.RemoveStatus(status);
            _statusEffects.Remove(status);
            status.OnExpire();
        }

        public void Tick()
        {
            if (_statusEffects.Count <= 0)
                return;
            
            foreach (var status in _statusEffects)
            {
                // status.UpdateTimer();

                status.Tick();
            }
        }

        public void RemoveInactive()
        {
            if (_statusEffects.Count <= 0)
                return;
            
            for (int i = _statusEffects.Count - 1; i >= 0; i--)
            {
                if (_statusEffects[i].IsEnded) 
                    UnregisterStatusEffect(_statusEffects[i]);
            }
        }
    }
}