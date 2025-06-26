using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using Abilities.MV;
using Services;
using Units;
using UnityEngine;

namespace Animations
{
    public class AnimationProcessingService
    {
        private Unit _source;
        private Unit _target;
        private IAbilityApplicatorService _abilityApplicatorService;
        private ICoroutineRunner _coroutineRunner;

        public AnimationProcessingService(IAbilityApplicatorService abilityApplicatorService, ICoroutineRunner coroutineRunner)
        {
            _abilityApplicatorService = abilityApplicatorService;
            _coroutineRunner = coroutineRunner;
        }

        public void PlayAnimation(Unit target, Unit source, AbilityModel abilityModel)
        {
            _source = source;
            _target = target;
            _source.Ready += OnReady;
            
            switch (abilityModel.AbilityType)
            {
                case AbilityType.FireBall:
                    source.PlayFireballAttackAnimation();
                    
                    break;
                
                case AbilityType.FrostBall:
                    
                    break;
                
                case AbilityType.PoisonBall:
                    
                    break;
                
                case AbilityType.AlcoholBall:
                    
                    break;
                
                case AbilityType.CastSpell:
                    _coroutineRunner.StartCoroutine(Timer());
                    _coroutineRunner.StartCoroutine(Timer2());
                    
                    source.PlayCastAnimation();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnReady()
        {
            _abilityApplicatorService.Apply(_target);
            _source.Ready -= OnReady;
        }
        
        private IEnumerator Timer()
        {
            float time = 1.4f;
            
            float startTime = 0;

            while (startTime < time)
            {
                startTime += Time.deltaTime;
                
                yield return null;
            }

            _source.ImposedEffect.Play();
            //Instantiate(ImposedEffect, transform.position, Quaternion.identity).Play();
        }
        
        private IEnumerator Timer2()
        {
            float time = 1.8f;
            
            float startTime = 0;

            while (startTime < time)
            {
                startTime += Time.deltaTime;
                
                yield return null;
            }
            
            _source.ExplosionEffect.Play();
            //ExplosionEffect.Play();
        }
    }
}