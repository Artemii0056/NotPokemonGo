using AbilityNew.AbilityDefinition;

namespace AbilityNew.Scripts.Validation
{
    public interface IAbilityValidator
    {
        AbilityValidationResult Validate(AbilitySO ability, StepExecutorRegistry registry);
    }
}