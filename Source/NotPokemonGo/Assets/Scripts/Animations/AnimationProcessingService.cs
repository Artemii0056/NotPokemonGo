using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure;
using Units;

namespace Animations
{
    public class AnimationProcessingService
    {
        private readonly Dictionary<AbilityType, Action<Unit>> _animationMap;

        public AnimationProcessingService()
        {
            _animationMap = new Dictionary<AbilityType, Action<Unit>>
            {
                { AbilityType.FireBall, PlayFireball },
                { AbilityType.FrostBall, PlayFrostBall },
                { AbilityType.PoisonBall, PlayPoisonBall },
                { AbilityType.CastSpell, PlayCastSpell }
            };
        }

        public void PlayAnimation(Unit source, AbilityModel abilityModel)
        {
            if (_animationMap.TryGetValue(abilityModel.AbilityType, out var playAnimation))
            {
                playAnimation.Invoke(source);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(abilityModel.AbilityType), $"No animation mapped for {abilityModel.AbilityType}");
            }
        }

        private void PlayFireball(Unit source) =>
            source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.FireballAttack);

        private void PlayFrostBall(Unit source) =>
            source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.CastSpell);

        private void PlayPoisonBall(Unit source) =>
            source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.RadialAttack);

        private void PlayCastSpell(Unit source) =>
            source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.CastSpell);
        
       // public void PlayAnimation(Unit source, AbilityModel abilityModel)
        // {
        //     switch (abilityModel.AbilityType)
        //     {
        //         case AbilityType.FireBall:
        //             source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.FireballAttack);
        //             break;
        //         
        //         case AbilityType.FrostBall:
        //             
        //             break;
        //         
        //         case AbilityType.PoisonBall:
        //             
        //             break;
        //         
        //         case AbilityType.AlcoholBall:
        //             
        //             break;
        //         
        //         case AbilityType.CastSpell:
        //             source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.CastSpell);
        //             break;
        //         
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        // }
    }
}