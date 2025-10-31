using UnityEngine;

namespace TimeServices
{
    public class TimeService : ITimeService //TODO Вот тут надо прописать другой TimeService
    {
        public float StandardTime => Time.unscaledDeltaTime;
        
        public float HalfTime => Time.unscaledDeltaTime / 2;
        
        public float QuarterTime => Time.unscaledDeltaTime / 4;
    }
}