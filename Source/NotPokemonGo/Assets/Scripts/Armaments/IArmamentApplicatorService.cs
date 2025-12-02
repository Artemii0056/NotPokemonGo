using Units;

namespace Armaments
{
	public interface IArmamentApplicatorService
	{
		void Apply(ArmamentSetup setup, Unit source,params Unit[] targets);
		void Apply(Armament armament);
	}
}