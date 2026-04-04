using AbilityNew.AbilityDefinition;

namespace AbilityNew.Scripts.Validation
{
    public interface IAbilityValidator
    {
        AbilityValidationResult Validate(AbilitySo ability, StepExecutorRegistry registry);
    }
}