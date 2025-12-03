using Units;

namespace Castaments
{
    public interface ICastamentApplicator
    {
        void Apply(CastamentSetup setup,  Unit source, params Unit[] targets);
    }
}