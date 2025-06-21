using Abilities.MV;
using Characters;
using Effects;
using Services.StaticDataServices;
using Statuses;
using Units;
using UnityEngine;

namespace Factories
{
    public class UnitFactory
    {
        private readonly Unit _prefab;
        private readonly EffectResolver _effectResolver;
        private readonly StaticDataLoadService _staticDataLoadService;

        public UnitFactory(Unit prefab, EffectResolver effectResolver, StaticDataLoadService staticDataLoadService)
        {
            _prefab = prefab;
            _effectResolver = effectResolver;
            _staticDataLoadService = staticDataLoadService;
        }

        public Unit Create(CharacterStaticData config, PlatoonType platoonType)
        {
            Unit unit = Object.Instantiate(_prefab);
            unit.Initialize(config.Stats, _effectResolver, platoonType, _staticDataLoadService);
            
            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            return unit;
        }
    }
}