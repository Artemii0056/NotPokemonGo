using Castaments;
using Units;

namespace Abilities
{
    public interface ICastamentApplicatorService
    {
        void Apply(CastamentSetup setup,  Unit source, params Unit[] targets);
    }
}