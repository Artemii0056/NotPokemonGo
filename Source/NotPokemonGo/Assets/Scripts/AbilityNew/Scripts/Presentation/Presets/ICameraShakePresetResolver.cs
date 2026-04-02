namespace AbilityNew.Scripts.Presentation.Presets
{
    public interface ICameraShakePresetResolver
    {
        CameraShakeData Resolve(CameraShakePreset preset);
    }
}