using System.Collections.Generic;
using System.Linq;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Executors.Debugger;
using AbilityNew.Scripts.Executors.Flow;
using AbilityNew.Scripts.Executors.Gameplay;
using AbilityNew.Scripts.Executors.Presentation;
using AbilityNew.Scripts.Presentation;
using AbilityNew.Scripts.Presentation.AbilityNew.Scripts.Presentation;
using AbilityNew.Scripts.Presentation.Executors;
using AbilityNew.Scripts.Presentation.Presets;
using AbilityNew.Scripts.Steps.Gameplay;
using Effects;
using QteSystem;
using Services.AudioServices;
using Services.Cameras;
using Spawners;
using Spawners.Spawner;
using Units;
using Units.Movement;

namespace Abilities
{
    public sealed class AbilityStepExecutorRegistryFactory : IAbilityStepExecutorRegistryFactory
    {
        private readonly IQteService _qteService;
        private readonly IArmamentSpawner _armamentSpawner;
        private readonly IEffectResolver _effectResolver;
        private readonly ITargetSelector _targetSelector;
        private readonly ICameraShakeService _cameraShakeService;
        private readonly IAudioService _audioService;
        private readonly IParticleSpawner _particleSpawner;

        public AbilityStepExecutorRegistryFactory(
            IQteService qteService, 
            IArmamentSpawner armamentSpawner,
            IEffectResolver effectResolver, 
            ITargetSelector targetSelector, 
            ICameraShakeService cameraShakeService, 
            IAudioService audioService, 
            IParticleSpawner particleSpawner)
        {
            _qteService = qteService;
            _armamentSpawner = armamentSpawner;
            _effectResolver = effectResolver;
            _targetSelector = targetSelector;
            _cameraShakeService = cameraShakeService;
            _audioService = audioService;
            _particleSpawner = particleSpawner;
        }
        
        public AbilityPresentationService PresentationService { get; private set; }

        public StepExecutorRegistry Create(SignalService signalService)
        {
            IUnitMover unitMover = new UnitMover();
            List<IAbilityStepExecutor> executors = CreateExecutors(signalService, unitMover).ToList();
             StepExecutorRegistry registry = new(executors);

            registry.AddExecutor(new BranchExecutor(registry));
            registry.AddExecutor(new ParallelExecutor(registry));
            registry.AddExecutor(new RepeatExecutor(registry));
            registry.AddExecutor(new SequenceExecutor(registry));
            registry.AddExecutor(new ResolveQteExecutor(registry));
            registry.AddExecutor(new EmitPresentationSignalExecutor(CreatePresentation()));

            return registry;
        }

        private AbilityPresentationService CreatePresentation()
        {
            List<IPresentationStepExecutor> presentationStepExecutors = new();
            presentationStepExecutors.Add(new CameraShakeStepExecutor(_cameraShakeService, new CameraShakePresetResolver()));
            presentationStepExecutors.Add(new PlayAudioStepExecutor(_audioService));
            presentationStepExecutors.Add(new SpawnParticleStepExecutor(_particleSpawner));
            
            PresentationStepExecutorRegistry presentationStepExecutorRegistry = 
                new PresentationStepExecutorRegistry(presentationStepExecutors);

            AbilityPresentationService abilityPresentationService =
                new AbilityPresentationService(presentationStepExecutorRegistry);
            
            PresentationService = abilityPresentationService;

            return abilityPresentationService;
        }

        private IEnumerable<IAbilityStepExecutor> CreateExecutors(
            SignalService signalService,
            IUnitMover unitMover)
        {
            return new IAbilityStepExecutor[]
            {
                new PlayAnimationExecutor(),
                new WaitSignalExecutor(signalService),
                new WaitingExecutor(),
                new StartQteExecutor(_qteService),
                new MoveExecutor(unitMover),
                new MoveBackExecutor(unitMover),
                new SpawnProjectileExecutor(_armamentSpawner),
                new ArmamentMoverExecutor(),
                new DamageStepExecutor(_effectResolver, _targetSelector),
                new SetBlackboardBoolExecutor(),
                new PrepareArmamentSpawnPointsExecutor(),
                new DebugExecutor(),
                new RequestCounterAttackExecutor(),
                new InitQteSeriesResultExecutor(),
                new AppendLastQteResultExecutor(),
            };
        }
    }
}