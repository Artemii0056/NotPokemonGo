using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityViewFactory
    {
        private readonly EffectManager _effectManager;

        public AbilityViewFactory(EffectManager effectManager) => 
            _effectManager = effectManager;

        public AbilityView Create(Vector3 position, Ability ability, AbilityView abilityConfigPrefab, Unit targetUnit)
        {
            AbilityView abilityView = Object.Instantiate(abilityConfigPrefab, position, Quaternion.identity);
            abilityView.Initialize(ability, targetUnit, _effectManager);
            
            return abilityView;
        }
    }
}