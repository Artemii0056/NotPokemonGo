using Effects;
using Units;

namespace Statuses
{
    public class Status 
    {
        private readonly IEffectResolver _effectResolver;

        public Status(StatusSetup setup, Unit source, Unit target, IEffectResolver effectResolver)
        {
            Setup = setup;
            Source = source;
            Target = target;
            _effectResolver = effectResolver;
        }
        
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
            EffectInfo damageInfo = new EffectInfo(Setup.EffectSetup.Value, Setup.EffectSetup.TargetType, Setup.EffectSetup.Type, Setup.EffectSetup.DamageType);
            _effectResolver.ApplyEffect(Source,Target, damageInfo);
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