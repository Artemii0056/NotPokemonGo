namespace AbilityNew.Scripts.Presentation
{
    public interface ICameraShakePresetResolver
    {
        CameraShakeData Resolve(CameraShakePreset preset);
    }
}