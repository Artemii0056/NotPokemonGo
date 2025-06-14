using Characters.Configs.Stats;
using Units;
using UnityEngine;

namespace Characters.Configs.Statuses
{
    public class HealStatus : Status
    {
        private readonly Unit _source;
        private readonly DamageResolver _damageResolver;

        public HealStatus(StatusSetup setup, Unit target, Unit source, DamageResolver damageResolver)
        {
            TickCount = setup.TickCount;
            Setup = setup;
            Target = target;
            _source = source;
            _damageResolver = damageResolver;

            TargetTime = setup.TargetTime;
        }

        public override void OnApply()
        {
            Debug.Log("Activate HealEffect");
        }

        public override void OnTick()
        {
            var damageInfo = new DamageInfo(DamageType.Heal, Setup.EffectSetup.Value, _source );
            
            float value =  _damageResolver.CalculateFinalDamage(Target, damageInfo);
            
            Target.ChangeValue(StatType.Health, value);
        }

        public override void OnExpire()
        {
            Debug.Log("Deativate HealEffect");
        }
    }
}