using Effects;
using Stats;
using Units;

namespace Statuses
{
    public class DamageStatus : Status
    {
        private readonly EffectResolver _effectResolver;

        public DamageStatus(StatusSetup setup, Unit target, EffectResolver effectResolver)
        {
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _effectResolver = effectResolver;

            TargetTime = setup.TargetTime;
        }

        public override void OnTick()
        {
            EffectInfo damageInfo = new EffectInfo(EffectType.Damage, Setup.EffectSetup.Value);

            float value = _effectResolver.CalculateFinalValue(Target, damageInfo);

            Target.ChangeValue(StatType.Health, value);
        }
    }
}