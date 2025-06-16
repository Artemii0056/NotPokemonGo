using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityViewFactory
    {

        public AbilityView Create(Vector3 position, AbilityView abilityConfigPrefab, Unit targetUnit)
        {
            AbilityView abilityView = Object.Instantiate(abilityConfigPrefab, position, Quaternion.identity);
            abilityView.Initialize(targetUnit);
            
            return abilityView;
        }
    }
}