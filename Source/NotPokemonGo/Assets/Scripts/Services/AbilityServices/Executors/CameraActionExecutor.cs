using System;
using Abilities.Configs;
using Abilities.Runtime;
using Cameras;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class CameraActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly ICameraService _camera;

        public CameraActionExecutor(ICameraService camera)
        {
            _camera = camera;
        }

        public bool CanExecute(PhaseSignalAction action) => action != null && action.HasCamera;

        public bool Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseFinishGate finishGate, Action tryCompleteFinish)
        {
            if (_camera == null)
                return false;

            var token = finishGate.Acquire();

            void OnComplete()
            {
                token.Dispose();
                tryCompleteFinish?.Invoke();
            }

            _camera.Play(action.CameraCommand, source, target, action.CameraBlendTimeout, OnComplete);
            return true;
        }
    }
}
