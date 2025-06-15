using Effects;
using Stats;
using Units;
using UnityEngine;

namespace Statuses
{
    public class HealStatus : Status
    {
        private readonly EffectResolver _effectResolver;

        public HealStatus(StatusSetup setup, Unit target, EffectResolver effectResolver)
        {
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _effectResolver = effectResolver;

            TargetTime = setup.TargetTime;
        }

        public override void OnApply()
        {
            Debug.Log("Activate HealStatus");
        }

        public override void OnTick()
        {
            var damageInfo = new EffectInfo(EffectType.Heal, Setup.EffectSetup.Value );
            
            float value =  _effectResolver.CalculateFinalValue(Target, damageInfo);
            
            Target.ChangeValue(StatType.Health, value);
        }

        public override void OnExpire()
        {
            Debug.Log("Deativate HealStatus");
        }
    }
}