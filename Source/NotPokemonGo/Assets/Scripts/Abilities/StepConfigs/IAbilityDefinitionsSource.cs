using Abilities.MV;

namespace Abilities.StepConfigs
{
    public interface IAbilityDefinitionsSource
    {
        bool TryGet(AbilityType type, out AbilityDefinitionConfig definition);
    }
}
