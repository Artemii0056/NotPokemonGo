using UnityEngine;

namespace RealTimeTickServices
{
    public interface IRealTimeTickService
    {
        void TickRealTime(float deltaTime);
    }
}