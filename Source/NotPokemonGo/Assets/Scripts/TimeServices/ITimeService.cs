namespace TimeServices
{
    public interface ITimeService
    {
        float StandardTime { get; }
        float HalfTime { get; }
        float QuarterTime { get; }
    }
}