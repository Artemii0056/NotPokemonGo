using Abilities.MV;

namespace Abilities
{
    public interface IAbilityProvider
    {
        AbilityModel AbilityModel { get; }
        void Remember(AbilityModel abilityModel);
        void Discard();
    }
}