namespace AbilityNew.AbilityDefinition
{
    public sealed class AbilityExecutionResult
    {
        //private readonly List<BattleEvent> _events = new();

        //public IReadOnlyList<BattleEvent> Events => _events;

        public bool Completed { get; private set; }
        public bool Interrupted { get; private set; }
        public bool Cancelled { get; private set; }

        //public void AddEvent(BattleEvent battleEvent) => _events.Add(battleEvent);

        public void MarkCompleted() => Completed = true;
        public void MarkInterrupted() => Interrupted = true;
        public void MarkCancelled() => Cancelled = true;
    }
}
/*Зачем это нужно

После ability battle flow должен понимать:

каст завершён или нет

был interrupt или нет

какие battle events произошли

что показывать в UI/log/reaction systems*/