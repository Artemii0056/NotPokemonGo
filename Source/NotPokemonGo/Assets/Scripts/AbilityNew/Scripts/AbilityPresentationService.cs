using System.Collections.Generic;
using AbilityNew.Presentation;
using AbilityNew.Scripts.Executors;
using AbilityNew.Scripts.Presentation;
using Services.AudioServices;
using Services.Cameras;
using Spawners;

namespace AbilityNew.Scripts
{
    public sealed class AbilityPresentationService : IAbilityPresentationService
    {
        private readonly IParticleSpawner _particleSpawner;
        private readonly IAudioService _audioService;
        private readonly ICameraShakeService _cameraShakeService;

        private readonly Dictionary<AbilitySO, AbilityPresentationConfig> _configs = new();

        public AbilityPresentationService(
            IParticleSpawner particleSpawner,
            IAudioService audioService,
            ICameraShakeService cameraShakeService)
        {
            _particleSpawner = particleSpawner;
            _audioService = audioService;
            _cameraShakeService = cameraShakeService;
        }

        public void Register(AbilityPresentationConfig config)
        {
            if (config == null || config.Ability == null)
                return;

            _configs[config.Ability] = config;
        }

        public void Play(AbilityPresentationContext context)
        {
            if (context == null || context.Ability == null)
                return;

            if (_configs.TryGetValue(context.Ability, out var config) == false)
                return;

            for (int i = 0; i < config.Entries.Count; i++)
            {
                var entry = config.Entries[i];
                if (entry.Signal != context.Signal)
                    continue;

                ExecuteActions(entry.Actions, context);
            }
        }

        private void ExecuteActions(List<PresentationAction> actions, AbilityPresentationContext context)
        {
            if (actions == null)
                return;

            for (int i = 0; i < actions.Count; i++)
            {
                ExecuteAction(actions[i], context);
            }
        }

        private void ExecuteAction(PresentationAction action, AbilityPresentationContext context)
        {
            switch (action.Type)
            {
                case PresentationActionType.SpawnParticleOnCaster:
                    if (context.Caster != null)
                        _particleSpawner.Spawn(context.Caster, action.SpawnType, action.ParticlePrefab);
                    break;

                case PresentationActionType.SpawnParticleOnTarget:
                    if (context.Target != null)
                        _particleSpawner.Spawn(context.Target, action.ParticlePrefab);
                    break;

                case PresentationActionType.PlaySoundOnCaster:
                    if (context.Caster != null)
                        _audioService.Play3D(action.AudioClip, context.Caster.transform.position);
                    break;

                case PresentationActionType.PlaySoundOnTarget:
                    if (context.Target != null)
                        _audioService.Play3D(action.AudioClip, context.Target.transform.position);
                    break;

                case PresentationActionType.CameraShake:
                    _cameraShakeService.Shake(
                        action.ShakeAmplitude,
                        action.ShakeFrequency,
                        action.ShakeDuration);
                    break;
            }
        }
    }
}