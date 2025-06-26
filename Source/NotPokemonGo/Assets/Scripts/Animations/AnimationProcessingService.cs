using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure;
using Services;
using Units;
using UnityEngine;

namespace Animations
{
    public class AnimationProcessingService
    {
        public void PlayAnimation(Unit target, Unit source, AbilityModel abilityModel)
        {
            switch (abilityModel.AbilityType)
            {
                case AbilityType.FireBall:
                    source.AbilityAnimationControllerBase.Play(Constants.AnimationsName.Mage.CastSpell);
                    break;
                
                case AbilityType.FrostBall:
                    
                    break;
                
                case AbilityType.PoisonBall:
                    
                    break;
                
                case AbilityType.AlcoholBall:
                    
                    break;
                
                case AbilityType.CastSpell:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}