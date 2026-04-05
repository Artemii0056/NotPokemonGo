using Abilities.MV;
using Units;

namespace Abilities
{
    public interface IAbilityExecutionSessionFactory
    {
        AbilityExecutionSession Create(
            Unit source,
            Unit target,
            AbilityModel abilityModel);
    }
}