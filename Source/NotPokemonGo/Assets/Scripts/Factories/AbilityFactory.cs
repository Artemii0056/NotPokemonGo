using System.Collections.Generic;
using Abilities;
using Statuses;
using Units;

namespace Factories
{
    public class AbilityFactory
    {
        private StatusFactory _statusFactory;

        public AbilityFactory(StatusFactory statusFactory) => 
            _statusFactory = statusFactory;

        public Ability Create(AbilityConfig abilityConfig, params Unit[] targetUnits)
        {
            return new Ability(abilityConfig, targetUnits);
        }
    }
}