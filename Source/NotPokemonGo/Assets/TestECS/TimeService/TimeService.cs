using UnityEngine;

namespace TestECS.Gameplay.Code.Features.Movement.Systems
{
    public class TimeService : ITimeService
    {
        public float DeltaTime => Time.deltaTime;
    }
}