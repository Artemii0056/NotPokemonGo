using Abilities.MV;
using Units;

namespace Abilities
{
    public interface IAbilityApplicatorService
    {
        void Apply(params Unit[] targets);
    }
}