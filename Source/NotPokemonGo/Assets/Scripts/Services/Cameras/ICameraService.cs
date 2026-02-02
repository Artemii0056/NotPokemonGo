using System;
using Units;

namespace Services.Cameras
{
    public interface ICameraService
    {
       void Play(CameraCommand cmd, Unit source, Unit target, float blendTimeout, Action onComplete);
    }
}