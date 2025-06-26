using Abilities.MV;
using Animations;
using Characters;
using Effects;
using Services.StaticDataServices;
using UI;
using Units;
using UnityEngine;
using VContainer;

namespace Factories
{
    public class UnitFactory : IUnitFactory
    {
        private readonly IEffectResolver _effectResolver;
        private readonly IObjectResolver _objectResolver;
        private readonly IParticleSystemFactory _particleSystemFactory;
        private readonly IAbilityProvider _abilityProvider;
        private readonly IStaticDataService _staticDataService;

        public UnitFactory(
            IEffectResolver effectResolver, 
            IObjectResolver  objectResolver,
            IParticleSystemFactory particleSystemFactory,
            IAbilityProvider abilityProvider,
            IStaticDataService staticDataService)
        {
            _effectResolver = effectResolver;
            _objectResolver = objectResolver;
            _particleSystemFactory = particleSystemFactory;
            _abilityProvider = abilityProvider;
            _staticDataService = staticDataService;
        }

        public Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType)
        {
            var posotion = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z);
            
            Unit unit = Object.Instantiate(config.Prefab, posotion, Quaternion.identity);
            
            unit.transform.SetParent(parentPosition, false);
            
            unit.Construct(config.Stats, _effectResolver, platoonType, _particleSystemFactory, _abilityProvider, _staticDataService);
            
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
            statusViewPanel.Construct(unit);
            _objectResolver.Inject(statusViewPanel);
        }
    }
}