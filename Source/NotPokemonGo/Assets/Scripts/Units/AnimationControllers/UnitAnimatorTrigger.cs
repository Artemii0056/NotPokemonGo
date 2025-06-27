using System;
using Abilities;
using Services.StaticDataServices;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class UnitAnimatorTrigger : IDisposable
    {
        //часть ищз юнита
        //абилити апликэйш сервис
        //мьюзик сервис
        // партиклы
        private IStaticDataService _staticDataService; 
        private IAbilityProvider _abilityProvider;
        private IParticleSystemFactory _particleSystemFactory;
        private IAbilityApplicatorService _abilityApplicatorService;
        private ITargetSelector _targetSelector;
        
        private AbilityAnimationControllerBase _controller;
        private Unit _unit;

        private int _abilityCount;

        public UnitAnimatorTrigger(
            Unit unit, 
            IStaticDataService staticDataService, 
            IAbilityProvider abilityProvider, 
            AbilityAnimationControllerBase controller, 
            IParticleSystemFactory particleSystemFactory,
            IAbilityApplicatorService abilityApplicatorService, 
            ITargetSelector targetSelector)
        {
            _unit = unit;
            _staticDataService = staticDataService;
            _abilityProvider = abilityProvider;
            _controller = controller;
            _particleSystemFactory = particleSystemFactory;
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;

            _controller.ParticleSystem1Started += OnParticleSystem1Started;
            _controller.ParticleSystem2Started += OnParticleSystem2Started;
            _controller.ParticleSystem3Started += OnParticleSystem3Started;
            
            _controller.Attack1Started += OnAttackStarted;
            
            _controller.Finished += OnFinished;

            _abilityCount = 0;
        }

        public void Dispose()
        {
            _controller.ParticleSystem1Started -= OnParticleSystem1Started;
            _controller.ParticleSystem2Started -= OnParticleSystem2Started;
            _controller.ParticleSystem3Started -= OnParticleSystem3Started;
            
            _controller.Attack1Started -= OnAttackStarted;
            
            _controller.Finished -= OnFinished;
        }

        private AbilityConfig SearchAbility() =>
            _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);

        private void OnParticleSystem1Started()
        {
            Debug.Log("OnParticleSystem1Started");
            
            _particleSystemFactory.Create(SearchAbility().StartAnimationParticles, _unit.transform.position,
                Quaternion.identity);
        }

        private void OnParticleSystem2Started()
        {
            Debug.Log("OnParticleSystem2Started");
            
            _particleSystemFactory.Create(SearchAbility().MiddleAnimationParticles, _unit.transform.position,
                Quaternion.identity);
        }

        private void OnParticleSystem3Started()
        {
            Debug.Log("OnParticleSystem3Started");
            
            _particleSystemFactory.Create(SearchAbility().EndAnimationParticles, _unit.transform.position,
                Quaternion.identity);
        }

        private void OnAttackStarted()
        {
            _abilityApplicatorService
                .Apply(_targetSelector.GetTargets(_abilityProvider.AbilityModel.TargetMode)
                    .ToArray());
        }

        private void OnFinished()
        {
            _abilityCount = 0;
        }
    }
}