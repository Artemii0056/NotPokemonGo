using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts;

namespace Abilities
{
    public interface IAbilityStepExecutorRegistryFactory
    {
        StepExecutorRegistry Create(SignalService signalService);
        AbilityPresentationService PresentationService { get; }
    }
}