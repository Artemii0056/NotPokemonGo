using Abilities.MV;
using Characters;
using Effects;
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

        public UnitFactory(IEffectResolver effectResolver, IObjectResolver  objectResolver)
        {
            _effectResolver = effectResolver;
            _objectResolver = objectResolver;
        }

        public Unit Create(Vector3 spawnPosition, Transform parentPosition, CharacterConfig config, PlatoonType platoonType)
        {
            Unit unit = Object.Instantiate(config.Prefab, spawnPosition, Quaternion.identity);
            
            unit.transform.SetParent(parentPosition, false);
            
            unit.Initialize(config.Stats, _effectResolver, platoonType);
            
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