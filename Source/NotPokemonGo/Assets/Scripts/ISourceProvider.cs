using Units;

public interface ISourceProvider
{
    Unit Source { get; }
    void Remember(Unit unit);
    void Discard();
}