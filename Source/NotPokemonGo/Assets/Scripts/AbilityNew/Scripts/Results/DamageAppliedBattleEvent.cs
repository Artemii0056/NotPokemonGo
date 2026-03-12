using Units;

namespace AbilityNew.Scripts.Results
{
    public class DamageAppliedBattleEvent : BattleEvent
    {
        public DamageAppliedBattleEvent(Unit source, Unit target, float value)
        {
            Source = source;
            Target = target;
            Value = value;
        }

        public Unit Source { get; }
        public Unit Target { get; }
        public float Value { get; }
    }
}