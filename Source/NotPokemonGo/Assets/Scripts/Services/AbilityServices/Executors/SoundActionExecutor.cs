using Abilities.Configs;
using Services.AudioServices;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class SoundActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly IAudioService _audio;

        public SoundActionExecutor(IAudioService audio)
        {
            _audio = audio;
        }

        public bool CanExecute(PhaseSignalAction action)
            => action != null && action.HasSound;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate gate) 
        {
            if (_audio == null)
                return;

            if (action.Sfx2D)
            {
                _audio.Play2D(action.SfxClip, action.SfxVolume);
            }
            else
            {
                var owner = action.ParticleOwner == ParticleOwner.Target ? target : source;
                _audio.Play3D(action.SfxClip, owner.transform.position, action.SfxVolume);
            }
        }
    }
}