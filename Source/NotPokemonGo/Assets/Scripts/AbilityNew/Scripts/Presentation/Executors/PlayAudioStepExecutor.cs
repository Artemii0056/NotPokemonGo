using Services.AudioServices;

namespace AbilityNew.Scripts.Presentation.Executors
{
    public sealed class PlayAudioStepExecutor : PresentationStepExecutor<PlayAudioStep>
    {
        private readonly IAudioService _audioService;

        public PlayAudioStepExecutor(IAudioService audioService)
        {
            _audioService = audioService;
        }

        protected override void Execute(PlayAudioStep step, AbilityPresentationContext context)
        {
            if (step.Clip == null)
                return;

            switch (step.Anchor)
            {
                case PresentationAnchor.Caster:
                    if (context.Caster != null)
                        _audioService.Play3D(step.Clip, context.Caster.transform.position);
                    break;

                case PresentationAnchor.Target:
                    if (context.Target != null)
                        _audioService.Play3D(step.Clip, context.Target.transform.position);
                    break;

                case PresentationAnchor.ExplicitTransform:
                    if (context.ExplicitTransform != null)
                        _audioService.Play3D(step.Clip, context.ExplicitTransform.position);
                    break;
            }
        }
    }
}