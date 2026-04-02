using UnityEngine;

namespace AbilityNew.Scripts.Presentation.Presets
{
    public sealed class CameraShakePresetResolver : ICameraShakePresetResolver
    {
        public CameraShakeData Resolve(CameraShakePreset preset)
        {
            return preset switch
            {
                CameraShakePreset.Light => new CameraShakeData(1f, 1f, 0.15f),
                CameraShakePreset.Medium => new CameraShakeData(2f, 2f, 0.25f),
                CameraShakePreset.Heavy => new CameraShakeData(3f, 3f, 0.35f),
                CameraShakePreset.Critical => new CameraShakeData(4f, 3f, 0.4f),
                CameraShakePreset.Explosion => new CameraShakeData(5f, 2f, 0.5f),
                _ => new CameraShakeData(0, 0, 0)
            };
        }
    }
}