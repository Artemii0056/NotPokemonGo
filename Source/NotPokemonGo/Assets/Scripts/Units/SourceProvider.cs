namespace Units
{
    public class SourceProvider : ISourceProvider //TODO Delete?
    {
        public Unit Source { get; private set; }

        public void Remember(Unit unit) => 
            Source = unit;

        public void Discard() => 
            Source = null;
    }
}