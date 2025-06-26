using System;
using Abilities;
using Abilities.MV;
using Animations;
using Units;

namespace DefaultNamespace
{
    public class DefaultAbilityExecutor
    {
        private AnimationProcessingService _animationService;
        private IAbilityApplicatorService _abilityApplicatorService;

        public DefaultAbilityExecutor(
            AnimationProcessingService animationProcessingService, 
            IAbilityApplicatorService abilityApplicatorService)
        {
            _animationService = animationProcessingService;
            _abilityApplicatorService = abilityApplicatorService;
        }

        public void Execute(Unit source, Unit target, AbilityModel ability, Action onHitCallback)
        {
            // _animationService.PlayAnimation(source, ability, () =>
            // {
            //     // 🎯 В этот момент AbilityApplicatorService сделает всю магию
            //     _abilityApplicatorService.Apply();
            // });
        }
    }
}