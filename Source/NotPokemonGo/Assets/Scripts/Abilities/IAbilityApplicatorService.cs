using Abilities.MV;
using Units;

namespace Abilities
{
    public interface IAbilityApplicatorService
    {
        void Remember(AbilityModel abilityModel);
        void RememberSource(Unit unit);
        void Apply(params Unit[] targets);
    }
}