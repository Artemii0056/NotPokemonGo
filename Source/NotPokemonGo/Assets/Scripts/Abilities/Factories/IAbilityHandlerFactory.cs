using Abilities.Bennet;
using Abilities.MV;

namespace Abilities.Factories
{
	public interface IAbilityHandlerFactory
	{
		IAbilityHandler Create(AbilityModel model);
	}
}