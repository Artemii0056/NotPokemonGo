using Effects;
using Stats;
using Units;

namespace Statuses
{
    public class DamageStatus : Status
    {
        private readonly IEffectResolver _effectResolver;

        public DamageStatus(StatusSetup setup, Unit source, Unit target, IEffectResolver effectResolver)
        {
            Source = source;
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _effectResolver = effectResolver;

            IsRefreshed = setup.IsRefreshed;
            IsPermanent = setup.IsPermanent;
        }

        public override void OnTick()
        {
            EffectInfo damageInfo = new EffectInfo(Setup.EffectSetup.Value, StatType.Damage, EffectType.Damage, DamageType.None);
            _effectResolver.ApplyEffect(Source,Target, damageInfo);
        }
    }
}