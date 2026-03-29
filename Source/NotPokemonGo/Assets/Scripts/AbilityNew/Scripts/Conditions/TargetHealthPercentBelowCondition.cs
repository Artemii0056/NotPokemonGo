using System;
using AbilityNew.AbilityDefinition;
using Stats;

namespace AbilityNew.Scripts.Conditions
{
    [Serializable]
    public class TargetHealthPercentBelowCondition : AbilityCondition //TODO Тестовая 
    {
        public float Threshold = 0.3f;

        public override bool Evaluate(AbilityExecutionRuntime runtime)
        {
            var target = runtime.Context.Target;

            if (target == null || target.IsAlive == false)
                return false;

            float currentHp = target.GetStat(StatType.Health);
            float maxHp = target.GetStat(StatType.MaxHealth);

            if (maxHp <= 0f)
                return false;

            return currentHp / maxHp < Threshold;
        }
    }
}