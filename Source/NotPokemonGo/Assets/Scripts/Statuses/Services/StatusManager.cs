using System.Collections.Generic;
using System.Linq;
using RealTimeTickServices;
using UnityEngine;

namespace Statuses.Services
{
    public class StatusManager : IStatusManager, IRealTimeTickService
    {
        private List<Status> _perTurn = new List<Status>();
        private List<Status> _perUnitTurn = new List<Status>();
        private List<Status> _perRealTime = new List<Status>();

        public void RegisterStatus(Status status)
        {
            if (status.Setup.UpdateType == StatusUpdateType.Realtime)
                _perRealTime.Add(status);
            else if (status.Setup.UpdateType == StatusUpdateType.PerTurn)
                _perTurn.Add(status);
            else
                _perUnitTurn.Add(status);

            status.OnApply();
        }

        public void UnregisterStatus(Status status)
        {
            status.OnExpire();
            status.Target.RemoveStatus(status);

            if (status.Setup.UpdateType == StatusUpdateType.Realtime)
                _perRealTime.Remove(status);
            else if (status.Setup.UpdateType == StatusUpdateType.PerTurn)
                _perTurn.Remove(status);
            else
                _perUnitTurn.Remove(status);
        }

        public void TickTurn() => 
            PerTurn(_perTurn);

        public void TickUnitTurn() => 
            PerTurn(_perUnitTurn);

        public void TickRealTime(float deltaTime)
        {
            if (_perRealTime.Count <= 0)
                return;

            foreach (var status in _perRealTime.ToList())
            {
                status.Tick();

                if (status.IsRealtimeEnded)
                {
                    UnregisterStatus(status);
                }
            }
        }

        public void RemoveInactive()
        {
            // RemoveInactiveIn(_perRealTime);
            // RemoveInactiveIn(_perTurn);
            // RemoveInactiveIn(_perUnitTurn);
        }
        
        private void PerTurn(List<Status> perUnitTurn)
        {
            if (perUnitTurn.Count <= 0)
                return;

            foreach (var status in perUnitTurn)
            {
                status.Tick();
                status.OnTick();

                if (status.IsEnded) 
                    UnregisterStatus(status);
            }
        }

        private void RemoveInactiveIn(List<Status> statuses)
        {
            if (statuses.Count <= 0)
                return;

            for (int i = statuses.Count - 1; i >= 0; i--)
            {
                var status = statuses[i];

                if (status.IsEnded)
                    UnregisterStatus(status);
            }
        }
    }
}