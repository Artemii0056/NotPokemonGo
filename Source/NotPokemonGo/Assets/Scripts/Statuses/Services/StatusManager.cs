using System.Collections.Generic;
using UnityEngine;

namespace Statuses.Services
{
    public class StatusManager : IStatusManager
    {
       private List<Status> _statuses = new List<Status>();

        public void RegisterStatus(Status status)
        {
            _statuses.Add(status);
            status.OnApply();
        }

        public void UnregisterStatus(Status status)
        {
            status.Target.RemoveStatus(status);
            _statuses.Remove(status);
            status.OnExpire();
        }

        public void Tick()
        {
            if (_statuses.Count <= 0)
                return;
            
            foreach (var status in _statuses)
            {
                // status.UpdateTimer();

                status.Tick();
            }
        }

        public void RemoveInactive()
        {
            if (_statuses.Count <= 0)
                return;
            
            for (int i = _statuses.Count - 1; i >= 0; i--)
            {
                if (_statuses[i].IsEnded) 
                    UnregisterStatus(_statuses[i]);
            }
        }

        public void TickTurn()
        {
            Debug.Log("StatusManager::TickTurn");
        }
    }
}