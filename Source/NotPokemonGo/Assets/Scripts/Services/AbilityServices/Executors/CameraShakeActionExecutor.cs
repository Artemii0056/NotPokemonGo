using System;
using Abilities.Configs;
using Services.Cameras;
using Units;
using UnityEngine;

namespace Services.AbilityServices.Executors
{
    public sealed class CameraShakeActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly ICameraShakeService _shakeService;

        public CameraShakeActionExecutor(ICameraShakeService shakeService) => 
            _shakeService = shakeService;

        public bool CanExecute(PhaseSignalAction action) =>
            action != null && action.HasShake;

        public void Execute(
            AbilityPhase phase,
            PhaseSignalAction action,
            Unit source,
            Unit target,
            PhaseGate finishGate,
            Action tryCompleteFinish)
        {
            if (_shakeService == null)
                return ;
            
           // Debug.Log("Camera shake action executed");

            //var token = finishGate.Acquire();
            //bool done = false;

            // void OnComplete()
            // {
            //     if (done) 
            //         return;
            //     
            //     done = true;
            //     token.Dispose();
            //     tryCompleteFinish?.Invoke();
            // }

            _shakeService.Shake(action.ShakeAmplitude, action.ShakeFrequency, action.ShakeDuration);
        }
    }
}