using System.Collections.Generic;
using AbilityNew.Scripts.Results;

namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionResult
    {
        private readonly List<BattleEvent> _events = new();

        public IReadOnlyList<BattleEvent> Events => _events;

        public bool Completed { get; private set; }
        public bool Interrupted { get; private set; }
        public bool Cancelled { get; private set; }

        public void AddEvent(BattleEvent battleEvent)
        {
            if (battleEvent == null)
                return;

            _events.Add(battleEvent);
        }

        public void MarkCompleted() => Completed = true;
        public void MarkInterrupted() => Interrupted = true;
        public void MarkCancelled() => Cancelled = true;
    }
}