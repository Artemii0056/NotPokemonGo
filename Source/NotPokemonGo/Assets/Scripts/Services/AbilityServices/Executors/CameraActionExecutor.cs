using System;
using Abilities.Configs;
using Services.Cameras;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class CameraActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly ICameraService _camera;

        public CameraActionExecutor(ICameraService camera) => 
            _camera = camera;

        public bool CanExecute(PhaseSignalAction action) => 
            action != null && action.HasCamera;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate, Action tryCompleteFinish)
        {
            if (_camera == null)
                return;

            IDisposable token = finishGate.Acquire();

            bool done = false;

            void OnComplete()
            {
                if (done) 
                    return;
                
                done = true;

                token.Dispose();
                tryCompleteFinish?.Invoke();
            }

            _camera.Play(action.CameraCommand, source, target, action.CameraBlendTimeout, OnComplete);
        }
    }
}
