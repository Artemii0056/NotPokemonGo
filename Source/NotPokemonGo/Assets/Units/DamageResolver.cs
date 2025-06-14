using System;
using Characters.Configs.Stats;

namespace Units
{
    public class DamageResolver
    {
        public float CalculateFinalDamage(Unit target, DamageInfo damageInfo)
        {
            float value = damageInfo.Value;

            switch (damageInfo.Type)
            {
              case DamageType.Physical:
                    float armor = target.GetStat(StatType.ArmorChance);
                    value = -Math.Max(0, damageInfo.Value - armor);
                    break;
              
              case DamageType.Heal:
                  value = damageInfo.Value;
                  break;
            }
            
            return value;
        }
    }
}