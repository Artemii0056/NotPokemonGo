using Effects;
using Stats;
using Units;
using UnityEngine;

namespace Statuses
{
    public class HealStatus : Status
    {
        private readonly Unit _source;
        private readonly EffectResolver _effectResolver;

        public HealStatus(StatusSetup setup, Unit target, Unit source, EffectResolver effectResolver)
        {
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _source = source;
            _effectResolver = effectResolver;

            TargetTime = setup.TargetTime;
        }

        public override void OnApply()
        {
            Debug.Log("Activate HealEffect");
        }

        public override void OnTick()
        {
            var damageInfo = new EffectInfo(EffectType.Heal, Setup.EffectSetup.Value, _source );
            
            float value =  _effectResolver.CalculateFinalValue(Target, damageInfo);
            
            Target.ChangeValue(StatType.Health, value);
        }

        public override void OnExpire()
        {
            Debug.Log("Deativate HealEffect");
        }
    }
}