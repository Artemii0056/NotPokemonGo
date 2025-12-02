using Armaments;
using Castaments;
using Units;

namespace Abilities
{
    public interface IAbilityApplicatorService
    {
        void Apply(CastamentSetup setup,  Unit source, params Unit[] targets);
        void Apply(ArmamentSetup setup, Unit source,params Unit[] targets);
    }
}