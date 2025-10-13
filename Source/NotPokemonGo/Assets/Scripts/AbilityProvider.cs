using Abilities.MV;

public class AbilityProvider : IAbilityProvider
{
    public AbilityModel AbilityModel { get; private set; }

    public void Remember(AbilityModel abilityModel)
    {
        AbilityModel = abilityModel;
    }

    public void Discard()
    {
        AbilityModel = null;
    }
}