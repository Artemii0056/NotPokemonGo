using Abilities;
using Abilities.MV;
using Units;

namespace Factories
{
    public class AbilityFactory
    {
        private StatusFactory _statusFactory;

        public AbilityFactory(StatusFactory statusFactory) => 
            _statusFactory = statusFactory;

        public AbilityModel Create(AbilityConfig abilityConfig, params Unit[] targetUnits)
        {
            return new AbilityModel(abilityConfig);
        }
    }
}