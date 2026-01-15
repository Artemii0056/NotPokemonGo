using System;
using Units;

namespace Services
{
    public interface ICameraService
    {
        void FocusOnSource(Unit source, Unit target, float duration, Action onComplete);
        void FocusOnTarget(Unit source, Unit target, float duration, Action onComplete);
        void Reset(float duration, Action onComplete);
            /// <summary>
            /// Запускает действие камеры и вызывает onComplete, когда “можно продолжать фазу”.
            /// </summary>
            void Play(Abilities.Configs.CameraCommand cmd, Unit source, Unit target, float blendTimeout, Action onComplete);
        // shake обычно синхронный, но можно тоже сделать async
        void Shake(float intensity, float duration, Action onComplete);
    }
}