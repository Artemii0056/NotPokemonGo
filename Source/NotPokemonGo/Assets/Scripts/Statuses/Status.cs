using Units;
using UnityEngine;

namespace Statuses
{
    public abstract class Status
    {
        public string Name { get; protected set; }

        // public float СurrentTimer { get; protected set; }
        // public float TargetTime { get; protected set; }

        public float TickCount { get; protected set; }
        public StatusSetup Setup { get; protected set; }
        public Unit Target { get; protected set; }

        public bool IsPermanent { get; protected set; }
        public bool IsRefreshed { get; protected set; }

        public bool IsEnded => TickCount <= 0;

        public virtual void OnApply()
        {
            Debug.Log($"{GetType().Name} Activate Status");
        }

        public virtual void OnTick() { }

        public virtual void OnExpire()
        {
            Debug.Log($"{GetType().Name} Deativate Status");
        }

        public void Tick()
        {
            OnTick();
            TickCount--;
        }

        // public void UpdateTimer() => 
        //     СurrentTimer++;

        public void IncreaseTickCount(float tickCount) => 
            TickCount += tickCount;

        public void Refresh(Status status)
        {
            // СurrentTimer = 0;
            TickCount = status.TickCount;
            // TargetTime = status.TargetTime;
        }
    }
}