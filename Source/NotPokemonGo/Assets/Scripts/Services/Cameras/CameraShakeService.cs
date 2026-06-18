using System;
using Cinemachine;
using DG.Tweening;

namespace Services.Cameras
{
    public class CameraShakeService : ICameraShakeService
    {
        private readonly CinemachineBasicMultiChannelPerlin _noise;
        private Tween _tween;

        public CameraShakeService(ICameraProvider cameraProvider)
        {
        //_noise = cameraProvider.VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }


        public void Shake(float amplitude, float frequency, float duration, Action onComplete = null)
        {
            if (_noise == null)
            {
                onComplete?.Invoke();
                return;
            }

            _tween?.Kill();

            _noise.m_FrequencyGain = frequency;
            _noise.m_AmplitudeGain = 0f;

            _tween = DOTween.To(
                    () => _noise.m_AmplitudeGain,
                    x => _noise.m_AmplitudeGain = x,
                    amplitude,
                    duration * 0.15f
                )
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _tween = DOTween.To(
                            () => _noise.m_AmplitudeGain,
                            x => _noise.m_AmplitudeGain = x,
                            0f,
                            duration * 0.85f
                        )
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() => onComplete?.Invoke());
                    _noise.m_AmplitudeGain = 0f;
                });
        }
        
        
    }
}