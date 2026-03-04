using Abilities.Bennet;
using Abilities.MV;
using Units;

namespace Abilities.Flow
{
    public interface IAbilityHandlerFactory
    {
        IAbilityHandler Create(Unit source, Unit target, AbilityModel model);
    }
}
