using Effects;
using Stats;
using Units;

namespace Statuses
{
    public class HealStatus : Status
    {
        private readonly IEffectResolver _effectResolver;

        public HealStatus(StatusSetup setup, Unit target, IEffectResolver effectResolver)
        {
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _effectResolver = effectResolver;

            // TargetTime = setup.TargetTime;
            IsRefreshed = setup.IsRefreshed;
            IsPermanent = setup.IsPermanent;
        }

        public override void OnTick()
        {
            EffectInfo damageInfo = new EffectInfo(Setup.EffectSetup.Value, StatType.Health, EffectType.Heal );
            _effectResolver.ApplyEffect(Target, damageInfo);
        }
    }
}