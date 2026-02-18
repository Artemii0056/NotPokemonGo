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

        public void TickTurn()
        {
            //Debug.Log("classic status tick");

            if (_perTurn.Count <= 0)
                return;

            foreach (var status in _perTurn)
                status.Tick();
        }

        public void TickUnitTurn()
        {
            if (_perUnitTurn.Count <= 0)
                return;

            foreach (var status in _perUnitTurn)
                status.Tick();
        }

        public void TickRealTime(float deltaTime)
        {
            if (_perRealTime.Count <= 0)
                return;

            foreach (var status in _perRealTime.ToList())
            {
                status.Tick();

                if (status.IsEnded)
                {
                    UnregisterStatus(status);
                    Debug.Log("Realtime tick ended");
                }
            }
        }

        public void RemoveInactive()
        {
            RemoveInactiveIn(_perRealTime);
            RemoveInactiveIn(_perTurn);
            RemoveInactiveIn(_perUnitTurn);
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