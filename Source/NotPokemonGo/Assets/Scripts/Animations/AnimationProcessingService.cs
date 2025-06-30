using Abilities;
using Services.StaticDataServices;
using Units;

namespace Animations
{
    public class AnimationProcessingService : IAnimationProcessingService
    {
        private readonly IStaticDataService _staticDataService;

        public AnimationProcessingService(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        private int GetAnimationHash(AbilityType abilityType) =>
            _staticDataService.GetAbilityConfig(abilityType).AnimationHash;

        public void PlayAnimation(Unit source, AbilityType abilityType) => 
            source.unitAnimatorController.Play(GetAnimationHash(abilityType));
    }
}