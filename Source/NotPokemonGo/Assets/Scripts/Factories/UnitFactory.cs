using Abilities.MV;
using Characters;
using Effects;
using Units;
using UnityEngine;

namespace Factories
{
    public class UnitFactory
    {
        private readonly Unit _prefab;
        private readonly EffectResolver _effectResolver;

        public UnitFactory(Unit prefab, EffectResolver effectResolver)
        {
            _prefab = prefab;
            _effectResolver = effectResolver;
        }

        public Unit Create(CharacterStaticData config, PlatoonType platoonType)
        {
            Unit unit = Object.Instantiate(_prefab);
            unit.Initialize(config.Stats, _effectResolver, platoonType);
            
            for (int i = 0; i < config.AbilityConfigs.Count; i++)
            {
                unit.AddAbility(new AbilityModel(config.AbilityConfigs[i]));
            }

            return unit;
        }
    }
}