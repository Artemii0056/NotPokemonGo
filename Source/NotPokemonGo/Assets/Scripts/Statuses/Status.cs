using Units;

namespace Statuses
{
    public abstract class Status
    {
        public string Name { get; protected set; }


        public float TickCount { get; protected set; }
        public StatusSetup Setup { get; protected set; }
        public Unit Target { get; protected set; }
        public Unit Source { get; protected set; }

        public bool IsPermanent { get; protected set; }
        public bool IsRefreshed { get; protected set; }

        public bool IsEnded => TickCount <= 0;

        public virtual void OnApply()
        {
        }

        public virtual void OnTick()
        {
        }

        public virtual void OnExpire()
        {
        }

        public void Tick()
        {
            OnTick();
            TickCount--;
        }
        
        public void IncreaseTickCount(float tickCount) =>
            TickCount += tickCount;

        public void Refresh(Status status) =>
            TickCount = status.TickCount;
    }
}