using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Castaments;
using Characters;
using Platoons;
using ReactionSystems;
using Services.AbilityServices;
using Services.AudioServices;
using Services.Cameras;
using Services.IdServices;
using Spawners;
using Spawners.Spawner;
using Stats;
using TimeServices;
using UI;
using UI.Sliders;
using Units.AnimationControllers;
using Units.Movement;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Units
{
    public class UnitFactory : IUnitFactory
    {
        private readonly IObjectResolver _objectResolver;
        private readonly ICastamentApplicator _castamentApplicator;
        private readonly ITargetSelector _targetSelector;
        private readonly IReactionService _reactionService;
        private readonly IParticleSpawner _particleSpawner;
        private readonly ICameraService _cameraService;
        private readonly IIdService _idService;
        private readonly IAudioService _audioService;
        private readonly ITimeService _timeService;
        

        public UnitFactory(
            IObjectResolver objectResolver,
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IAbilityService abilityService,
            IReactionService reactionService,
            IArmamentSpawner spawner,
            IParticleSpawner particleSpawner,
            ICameraService cameraService, 
            IIdService  idService,
            IAudioService audioService)
        {
            _objectResolver = objectResolver;
            _castamentApplicator = castamentApplicator;
            _targetSelector = targetSelector;
            _reactionService = reactionService;
            _particleSpawner = particleSpawner;
            _cameraService = cameraService;
            _idService = idService;
            _audioService = audioService;

            _reactionService.Register(new ReflectFireballReaction(spawner)); // потом вынесем
            _reactionService.Register(new CounterattackReaction(abilityService));
        }

        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType)
        {
            Vector3 posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);

            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);

            unit.transform.SetParent(parentPosition, false);
            
            AnimatorController controller = unit.AnimatorController;

            UnitAnimatorTrigger unitAnimatorTrigger = new UnitAnimatorTrigger(
                unit,
                controller,
                new AbilityPhaseService(_castamentApplicator, _targetSelector, _particleSpawner, new UnitMover(), _cameraService, _audioService, _timeService));

            unit.SetAnimationTrigger(unitAnimatorTrigger);

            unit.Construct(config.Stats,  platoonType, _idService.GetNextId());

            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            InitializeView(unit);

            return unit;
        }
        
        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config,  PlatoonType platoonType, Dictionary<StatType, StatSetup> stats)
        {
            Vector3 posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);

            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);

            unit.transform.SetParent(parentPosition, false);
            
            unit.Construct(stats, platoonType, _idService.GetNextId());

            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            InitializeView(unit);

            return unit;
        }

        private void InitializeView(Unit unit)
        {
            UnitViewPanel unitViewPanel = unit.GetComponentInChildren<UnitViewPanel>();
             UnitSliderView slidersView = unit.GetComponentInChildren<UnitSliderView>();
             BillboardToCamera buildingToCamera = unit.GetComponentInChildren<BillboardToCamera>();
             
             UnitHudView unitHudView = unit.GetComponentInChildren<UnitHudView>();
             unitHudView.Bind(unit);
             
            // UnitDamageView unitDamageView = unit.GetComponentInChildren<UnitDamageView>();
            _objectResolver.Inject(unitViewPanel);
             _objectResolver.Inject(slidersView);
             _objectResolver.Inject(buildingToCamera);
            // _objectResolver.Inject(unitDamageView);

            unitViewPanel.Construct(unit);
        }
    }
}