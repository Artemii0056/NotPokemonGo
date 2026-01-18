namespace TimeServices
{
    public interface ITimeService
    {
        void HitStop(float timeScale, float duration);
        void SlowMo(float timeScale, float duration);
        
        float UnscaledDeltaTime { get; }
        float HalfTime { get; }
        float QuarterTime { get; }
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
    }
}