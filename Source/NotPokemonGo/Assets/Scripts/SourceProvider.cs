using Units;

public class SourceProvider : ISourceProvider
{
    public Unit Source { get; private set; }

    public void Remember(Unit unit) => 
        Source = unit;

    public void Discard() => 
        Source = null;
}