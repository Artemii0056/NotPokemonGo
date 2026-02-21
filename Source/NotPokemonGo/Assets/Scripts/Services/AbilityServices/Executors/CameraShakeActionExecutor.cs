using Abilities.Configs;
using Services.Cameras;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class CameraShakeActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly ICameraShakeService _shakeService;

        public CameraShakeActionExecutor(ICameraShakeService shakeService) => 
            _shakeService = shakeService;

        public bool CanExecute(PhaseSignalAction action) =>
            action != null && action.HasShake;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate)
        {
            if (_shakeService == null)
                return ;

            _shakeService.Shake(action.ShakeAmplitude, action.ShakeFrequency, action.ShakeDuration);
        }
    }
}