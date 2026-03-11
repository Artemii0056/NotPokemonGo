using Abilities.Signals;
using AbilityNew.Scripts;
using Units.AnimationControllers;
using UnityEngine;

namespace AbilityNew
{
    public class AnimationSignalRelay
    {
        private SignalService _signalService;
        private AnimatorController _animatorController;

        public AnimationSignalRelay(SignalService signalService, AnimatorController animatorController)
        {
            _signalService = signalService;
            _animatorController = animatorController;

            _animatorController.Signal += OnSignal;
        }

        public void EmitSignal(int signal)
        {
            Debug.Log(signal);
        
            if (_signalService == null)
                return;

            _signalService.Emit(PhaseSignalUtil.FromInt(signal));
        }

        private void OnSignal(int signal)
        {
            EmitSignal(signal);
        }
    }
}