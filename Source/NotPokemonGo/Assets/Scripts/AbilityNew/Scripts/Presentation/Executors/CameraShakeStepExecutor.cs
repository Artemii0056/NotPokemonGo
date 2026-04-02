using AbilityNew.Scripts.Presentation.Presets;
using Services.Cameras;

namespace AbilityNew.Scripts.Presentation.Executors
{
    public sealed class CameraShakeStepExecutor : PresentationStepExecutor<CameraShakeStep>
    {
        private readonly ICameraShakeService _cameraShakeService;
        private readonly ICameraShakePresetResolver _presetResolver;

        public CameraShakeStepExecutor(
            ICameraShakeService cameraShakeService,
            ICameraShakePresetResolver presetResolver)
        {
            _cameraShakeService = cameraShakeService;
            _presetResolver = presetResolver;
        }

        protected override void Execute(CameraShakeStep step, AbilityPresentationContext context)
        {
            var data = _presetResolver.Resolve(step.Preset);

            if (data.Duration <= 0f)
                return;

            _cameraShakeService.Shake(data.Amplitude, data.Frequency, data.Duration);
        }
    }
}