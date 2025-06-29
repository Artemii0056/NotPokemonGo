using System;
using Abilities;
using Abilities.MV;
using Animations;
using Characters;
using Effects;
using Services.StaticDataServices;
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
        private readonly IEffectResolver _effectResolver;
        private readonly IObjectResolver _objectResolver;
        private readonly IParticleSystemFactory _particleSystemFactory;
        private readonly IAbilityProvider _abilityProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly IAbilityApplicatorService _abilityApplicatorService;
        private ITargetSelector _targetSelector;

        public UnitFactory(
            IEffectResolver effectResolver, 
            IObjectResolver  objectResolver,
            IParticleSystemFactory particleSystemFactory,
            IAbilityProvider abilityProvider,
            IStaticDataService staticDataService, IAbilityApplicatorService abilityApplicatorService, ITargetSelector targetSelector)
        {
            _effectResolver = effectResolver;
            _objectResolver = objectResolver;
            _particleSystemFactory = particleSystemFactory;
            _abilityProvider = abilityProvider;
            _staticDataService = staticDataService;
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
        }

        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType)
        {
            Vector3 posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);
            
            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);
            
            unit.transform.SetParent(parentPosition, false);
            
            unit.Construct(config.Stats, _effectResolver, platoonType);
            
            UnitAnimatorController controller =unit.unitAnimatorController;
            
         //   AbilityAnimationControllerBase animationController = unit.AbilityAnimationControllerBase;
            
            UnitAnimatorTrigger unitAnimatorTrigger = new UnitAnimatorTrigger(
                unit, 
                _staticDataService, 
                _abilityProvider, 
                controller, 
                _particleSystemFactory, 
                _abilityApplicatorService,
                _targetSelector);
            
            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            InitializeView(unit);
            
            return unit;
        }

        private void InitializeView(Unit unit)
        {
            StatusViewPanel statusViewPanel = unit.GetComponentInChildren<StatusViewPanel>();
            UnitSliderView slidersView = unit.GetComponentInChildren<UnitSliderView>();
            
            statusViewPanel.Construct(unit);

            _objectResolver.Inject(statusViewPanel);
            _objectResolver.Inject(slidersView);
        }
    }

}