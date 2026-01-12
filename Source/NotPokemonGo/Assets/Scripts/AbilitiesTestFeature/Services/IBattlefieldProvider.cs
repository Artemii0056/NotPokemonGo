using Battlefields;

namespace AbilitiesTestFeature.Services
{
	public interface IBattlefieldProvider
	{
		void Remember(Battlefield battlefield);
		void Discard();
		Battlefield Get { get; }
	}
}