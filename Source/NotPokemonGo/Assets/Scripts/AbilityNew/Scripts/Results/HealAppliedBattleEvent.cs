using Units;

namespace AbilityNew.Scripts.Results
{
    public class HealAppliedBattleEvent : BattleEvent
    {
        public HealAppliedBattleEvent(Unit contextSource, Unit target, float effectValue)
        {
            ContextSource = contextSource;
            Target = target;
            EffectValue = effectValue;
        }

        private Unit Target { get; }
        public Unit ContextSource { get; }
        private float EffectValue { get; }
    }
}