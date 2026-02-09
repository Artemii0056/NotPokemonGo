using System;
using Units;

namespace Services.Cameras
{
    public interface ICameraShakeService
    {
        void Shake(float amplitude, float frequency, float duration, Action onComplete = null);
    }
}