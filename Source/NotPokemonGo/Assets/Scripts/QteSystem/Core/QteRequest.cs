using Units;

namespace QteSystem.Core
{
    public readonly struct QteRequest
    {
        public QteRequest(
            QteType type,
            Unit target,
            float duration,
            QteOutcomeMode outcomeMode)
        {
            Type = type;
            Target = target;
            Duration = duration;
            OutcomeMode = outcomeMode;
        }

        public QteType Type { get; }
        public Unit Target { get; }
        public float Duration { get; }
        public QteOutcomeMode OutcomeMode { get; }
    }
}