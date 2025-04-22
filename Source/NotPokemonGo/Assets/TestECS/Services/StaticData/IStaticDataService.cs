using TestECS.Gameplay.Features.Abilities;
using TestECS.Gameplay.Features.Abilities.Configs;

namespace TestECS.Services.StaticData
{
    public interface IStaticDataService
    {
        AbilityConfig GetAbilityById(AbilityId abilityId);
        AbilityLevel GetAbilityLevel(AbilityId abilityId, int level);
    }
}