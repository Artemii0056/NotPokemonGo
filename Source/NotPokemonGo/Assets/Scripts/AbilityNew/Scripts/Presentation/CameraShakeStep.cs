using System;
using AbilityNew.Scripts.Presentation.Presets;

namespace AbilityNew.Scripts.Presentation
{
    [Serializable]
    public sealed class CameraShakeStep : PresentationStep
    {
        public CameraShakePreset Preset;
    }
}