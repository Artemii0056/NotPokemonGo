using Units;
using UnityEngine;

namespace Statuses
{
    public abstract class Status
    {
        public string Name { get; protected set; }

        public float СurrentTimer { get; protected set; }
        public float TargetTime { get; protected set; }

        public float TickCount { get; protected set; }
        public StatusSetup Setup { get; protected set; }
        public Unit Target { get; protected set; }

        public bool IsReady => СurrentTimer >= TargetTime;

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
            СurrentTimer = 0;

            TickCount--;
            OnTick();

            if (TickCount <= 0)
            {
                OnExpire();
                Target.RemoveStatus(this);
            }
        }

        public void UpdateTimer(float time)
        {
            СurrentTimer += time;
        }
    }
}