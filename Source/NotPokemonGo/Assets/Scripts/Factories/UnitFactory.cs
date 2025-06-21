using Abilities.MV;
using Characters;
using Effects;
using Services.StaticDataServices;
using Statuses;
using Units;
using UnityEngine;

namespace Factories
{
    public class UnitFactory : IUnitFactory
    {
        private readonly IEffectResolver _effectResolver;
        private readonly IStaticDataService _staticDataLoadService;

        // public UnitFactory(Unit prefab, EffectResolver effectResolver, StaticDataLoadService staticDataLoadService)
        // {
        //     _prefab = prefab;
        //     _effectResolver = effectResolver;
        //     _staticDataLoadService = staticDataLoadService;
        // }
        
        public UnitFactory(IEffectResolver effectResolver, IStaticDataService staticDataLoadService)
        {
            _effectResolver = effectResolver;
            _staticDataLoadService = staticDataLoadService;
        }

        public Unit Create(Transform transform, Transform parentPosition, CharacterConfig config, PlatoonType platoonType)
        {
            Unit unit = Object.Instantiate(config.Prefab, transform.position, Quaternion.identity);
            unit.transform.SetParent(parentPosition);
            unit.Initialize(config.Stats, _effectResolver, platoonType, _staticDataLoadService);
            
            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            return unit;
        }
    }
}