using Units;

namespace Armaments
{
	public interface IArmamentApplicator
	{
		void Apply(ArmamentSetup setup, ArmamentFlyingType flyingType, Unit source, params Unit[] targets);
	}
}