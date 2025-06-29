using System;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Infrastructure;
using Units;
using UnityEngine;

namespace Animations
{
    public class AnimationProcessingService //Интерфейс и скормить юниту
    {
        private readonly Dictionary<AbilityType, Action<Unit>> _animationMap;

        public AnimationProcessingService()
        {
            _animationMap = new Dictionary<AbilityType, Action<Unit>>
            {
                { AbilityType.FireBall, PlayFireball },
                { AbilityType.FrostBall, PlayFrostBall },
                { AbilityType.PoisonBall, PlayPoisonBall },
                { AbilityType.CastSpell, PlayCastSpell },
                { AbilityType.DoubleAttack, PlayDoubleAttack },
                // TODO насрано базовой абилкой
                { AbilityType.BaseAbility, PlayCastSpell },
            };
        }

        public void PlayAnimation(Unit source, AbilityModel abilityModel)
        {
            if (source == null)
                Debug.LogError("source == null");

            if (abilityModel == null)
                Debug.LogError("abilityModel == null");

            if (_animationMap == null)
                Debug.LogError("_animationMap == null");
            
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
            source.unitAnimatorController.Play(Constants.AnimationsName.Mage.FireballAttack);

        private void PlayFrostBall(Unit source) =>
            source.unitAnimatorController.Play(Constants.AnimationsName.Mage.CastSpell);

        private void PlayPoisonBall(Unit source) =>
            source.unitAnimatorController.Play(Constants.AnimationsName.Mage.RadialAttack);

        private void PlayCastSpell(Unit source) =>
            source.unitAnimatorController.Play(Constants.AnimationsName.Mage.CastSpell);
        
        private void PlayDoubleAttack(Unit source) =>
            source.unitAnimatorController.Play(Constants.AnimationsName.Swordsman.DoubleAttack);
    }
}