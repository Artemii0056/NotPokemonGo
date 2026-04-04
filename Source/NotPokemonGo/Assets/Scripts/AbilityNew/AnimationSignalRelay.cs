using System;
using Abilities.Signals;
using AbilityNew.Scripts;
using Units.AnimationControllers;

namespace AbilityNew
{
    public sealed class AnimationSignalRelay : IDisposable
    {
        private readonly ISignalService _signalService;
        private readonly AnimatorController _animatorController;

        public AnimationSignalRelay(
            ISignalService signalService,
            AnimatorController animatorController)
        {
            _signalService = signalService ?? throw new ArgumentNullException(nameof(signalService));
            _animatorController = animatorController ?? throw new ArgumentNullException(nameof(animatorController));

            _animatorController.Signal += OnSignal;
        }

        public void Dispose()
        {
            _animatorController.Signal -= OnSignal;
        }

        private void OnSignal(int signalId)
        {
            PhaseSignal signal = PhaseSignalUtil.FromInt(signalId);
            _signalService.Emit(signal);
        }
    }
}