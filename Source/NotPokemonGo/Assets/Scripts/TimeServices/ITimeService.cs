namespace TimeServices
{
    public interface ITimeService
    {
        float UnscaledDeltaTime { get; }
        float HalfTime { get; }
        float QuarterTime { get; }
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
    }
}