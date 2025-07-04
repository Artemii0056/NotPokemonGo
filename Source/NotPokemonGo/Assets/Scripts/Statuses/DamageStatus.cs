using Effects;
using Stats;
using Units;

namespace Statuses
{
    public class DamageStatus : Status
    {
        private readonly IEffectResolver _effectResolver;

        public DamageStatus(StatusSetup setup, Unit target, IEffectResolver effectResolver)
        {
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _effectResolver = effectResolver;

            TargetTime = setup.TargetTime;
            IsRefreshed = setup.IsRefreshed;
            IsPermanent = setup.IsPermanent;
        }

        public override void OnTick()
        {
            EffectInfo damageInfo = new EffectInfo(EffectType.ModifyStat, Setup.EffectSetup.Value, StatType.Damage);

            float value = _effectResolver.CalculateFinalValue(Target, damageInfo);

            Target.ChangeStatValue(StatType.Health, value);
        }
    }
}