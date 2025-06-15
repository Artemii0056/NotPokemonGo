using Units;

namespace Statuses
{
    public abstract class Status
    {
        public float СurrentTimer { get; protected set; }
        public float TargetTime { get; protected set; }
        
        public string Name { get; protected set; }
        public float TickCount { get; protected set; }
        public StatusSetup Setup { get; protected set; }
        public Unit Target { get; protected set; }

        public bool IsReady => СurrentTimer >= TargetTime;

        public abstract void OnApply();
        public abstract void OnTick();
        public abstract void OnExpire();

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