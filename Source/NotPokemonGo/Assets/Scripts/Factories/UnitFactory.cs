using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Characters;
using ReactionSystems;
using Services;
using Services.StaticDataServices;
using Stats;
using UI;
using UI.Sliders;
using Units;
using Units.AnimationControllers;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Factories
{
    public class UnitFactory : IUnitFactory
    {
        private readonly IObjectResolver _objectResolver;
        private readonly IParticleSystemFactory _particleSystemFactory;
        private readonly IAbilityProvider _abilityProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private readonly ITargetSelector _targetSelector;
        private readonly IAbilityService _abilityService;
        private readonly IReactionService _reactionService;

        public UnitFactory(
            IObjectResolver objectResolver,
            IParticleSystemFactory particleSystemFactory,
            IAbilityProvider abilityProvider,
            IStaticDataService staticDataService,
            IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector,
            IAbilityService abilityService, 
            IReactionService reactionService)
        {
            _objectResolver = objectResolver;
            _particleSystemFactory = particleSystemFactory;
            _abilityProvider = abilityProvider;
            _staticDataService = staticDataService;
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
            _abilityService = abilityService;
            _reactionService = reactionService;
            
            _reactionService.Register(new CounterattackReaction(_abilityService)); //TODO ВЫПЫЛИТЬ ОТСЮДА! 
        }

        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType)
        {
            
            Vector3 posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);

            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);

            unit.transform.SetParent(parentPosition, false);
            
            UnitAnimatorController controller = unit.UnitAnimatorController;

            UnitAnimatorTrigger unitAnimatorTrigger = new UnitAnimatorTrigger(
                unit,
                _staticDataService,
                _abilityProvider,
                controller,
                _particleSystemFactory,
                _abilityApplicatorService,
                _targetSelector, 
                _reactionService);

            unit.SetAnumationTrigger(unitAnimatorTrigger);

            unit.Construct(config.Stats,  platoonType);

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
            
            unit.Construct(stats, platoonType);

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

            _objectResolver.Inject(unitViewPanel);
            _objectResolver.Inject(slidersView);

            unitViewPanel.Construct(unit);
        }
    }
}