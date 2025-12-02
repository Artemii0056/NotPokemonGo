using Armaments;
using Units;

namespace DodgeSystem
{
	public interface IDodgeService
	{
		Armament Dodge(Armament armament);
		bool CanDodge(Unit unit);
	}
}