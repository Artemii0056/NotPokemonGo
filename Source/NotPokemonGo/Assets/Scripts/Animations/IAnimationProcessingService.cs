using Abilities;
using Units;

namespace Animations
{
    public interface IAnimationProcessingService
    {
        void PlayAnimation(Unit source, AbilityType abilityType);
    }
}