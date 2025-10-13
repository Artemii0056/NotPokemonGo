using Abilities.MV;

public interface IAbilityProvider
{
    AbilityModel AbilityModel { get; }
    void Remember(AbilityModel abilityModel);
    void Discard();
}