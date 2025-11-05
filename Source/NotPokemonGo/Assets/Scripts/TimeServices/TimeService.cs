using UnityEngine;

namespace TimeServices
{
    public class TimeService : ITimeService //TODO Вот тут надо прописать другой TimeService
    {
        public float DeltaTime => Time.deltaTime;
        public float FixedDeltaTime => Time.fixedDeltaTime;
        public float StandardTime => Time.unscaledTime;
        public float HalfTime => Time.deltaTime / 2;
        public float QuarterTime => Time.deltaTime / 4;
    }
}