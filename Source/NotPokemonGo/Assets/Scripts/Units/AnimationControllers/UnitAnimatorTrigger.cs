using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Abilities;
using Services.StaticDataServices;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

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

        private Dictionary<AbilityType, AbilityAnchor> _anchors;
        private  List<ParticleSystem> _particles;

        private int _abilityCount;

        public UnitAnimatorTrigger(
            Unit unit,
            IStaticDataService staticDataService,
            IAbilityProvider abilityProvider,
            AbilityAnimationControllerBase controller,
            IParticleSystemFactory particleSystemFactory, IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector)
        {
            _unit = unit;
            _staticDataService = staticDataService;
            _abilityProvider = abilityProvider;
            _controller = controller;
            _particleSystemFactory = particleSystemFactory;
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
            _particles = new List<ParticleSystem>();

            _controller.ParticleSystem1Started += OnParticleSystem1Started;
            _controller.ParticleSystem2Started += OnParticleSystem2Started;
            _controller.ParticleSystem3Started += OnParticleSystem3Started;

            _controller.Attack1Started += OnAttackStarted;

            _controller.Finished += OnFinished;

            _abilityCount = 0;

            InitializeAnchors(_unit);
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

            var type = SearchAbility().AbilityType;

            if (_anchors.TryGetValue(type, out AbilityAnchor anchor))
            {
                var point = anchor.Transforms[0];

                List<ParticleSystem> a =_particleSystemFactory.Create(SearchAbility().StartAnimationParticles, point,
                    Quaternion.identity);
                
                _particles.AddRange(a);
            }
        }

        private void OnParticleSystem2Started()
        {
            Debug.Log("OnParticleSystem2Started");
            
            var type = SearchAbility().AbilityType;

            if (_anchors.TryGetValue(type, out AbilityAnchor anchor))
            {
                var point = anchor.Transforms[1];

                var a = _particleSystemFactory.Create(SearchAbility().MiddleAnimationParticles, point,
                    Quaternion.identity);
                
                _particles.AddRange(a);
            }
        }

        private void OnParticleSystem3Started()
        {
            Debug.Log("OnParticleSystem3Started");

            var a = _particleSystemFactory.Create(SearchAbility().EndAnimationParticles, _unit.transform,
                Quaternion.identity);
            
            _particles.AddRange(a);
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

            foreach (var particle in _particles.ToList())
            {
                UnityEngine.Object.Destroy(particle.gameObject);
                _particles.Remove(particle);
            }
        }

        private void InitializeAnchors(Unit unit)
        {
            _anchors = new();

            foreach (var anchor in unit.AbilityAnchors)
            {
                if (!_anchors.TryAdd(anchor.AbilityType, anchor))
                {
                    Debug.LogWarning($"Дубликат Anchor для {anchor.AbilityType} у {unit.name}");
                }
            }
        }
    }
}