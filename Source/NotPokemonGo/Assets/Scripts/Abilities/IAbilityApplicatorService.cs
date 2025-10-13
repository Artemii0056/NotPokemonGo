using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using Abilities.MV;
using Units;

namespace Abilities
{
    public interface IAbilityApplicatorService
    {
        void Apply(CastamentSetup setup, params Unit[] targets);
        void Apply(ArmamentSetup setup, params Unit[] targets);
    }
}