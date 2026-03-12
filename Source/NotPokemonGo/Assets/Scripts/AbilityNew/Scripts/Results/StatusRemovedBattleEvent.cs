using Statuses;
using Units;

namespace AbilityNew.Scripts.Results
{
    public class StatusRemovedBattleEvent : BattleEvent
    {
        public StatusRemovedBattleEvent(Unit source, Unit target, StatusType statusType)
        {
            Source = source;
            Target = target;
            StatusType = statusType;
        }

        public Unit Source { get; }
        public Unit Target { get; }
        public StatusType StatusType { get; }
    }
}