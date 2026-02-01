using System;
using Abilities.Configs;
using Units;

namespace Services
{
    public interface ICameraService
    {
       void Play(CameraCommand cmd, Unit source, Unit target, float blendTimeout, Action onComplete);
    }
}