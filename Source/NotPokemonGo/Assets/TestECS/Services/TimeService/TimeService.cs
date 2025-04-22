using UnityEngine;

namespace TestECS.TimeService
{
    public class TimeService : ITimeService
    {
        public float DeltaTime => Time.deltaTime;
    }
}