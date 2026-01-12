using Battlefields;

namespace AbilitiesTestFeature.Services
{
	public class BattlefieldProvider : IBattlefieldProvider
	{
		public Battlefield Get { get; private set; }

		public void Remember(Battlefield battlefield) => 
			Get	=  battlefield;

		public void Discard() => 
			Get	=  null;
	}
}