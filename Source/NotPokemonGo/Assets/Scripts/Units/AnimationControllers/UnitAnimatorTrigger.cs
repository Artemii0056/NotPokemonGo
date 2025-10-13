using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Assets;
using Effects;
using Services.AbilityServices;
using Services.StaticDataServices;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class UnitAnimatorTrigger : IDisposable
    {
        private IStaticDataService _staticDataService;
        private IAbilityProvider _abilityProvider;
        private IParticleSystemFactory _particleSystemFactory;
        
        private AbilityPhaseService _abilityPhaseService;

        private UnitAnimatorController _controller;
        private Unit _unit;

        private Dictionary<AbilityType, AbilityAnchor> _anchors;
        private  List<ParticleSystem> _particles;
        
        private AbilityPhase _phase;

        public event Action ActionEnded;
        
        public UnitAnimatorTrigger(
            Unit unit,
            IStaticDataService staticDataService,
            IAbilityProvider abilityProvider,
            UnitAnimatorController controller,
            IParticleSystemFactory particleSystemFactory, 
            IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector)
        {
            _unit = unit;
            _staticDataService = staticDataService;
            _abilityProvider = abilityProvider;
            _controller = controller;
            _particleSystemFactory = particleSystemFactory;

            _particles = new List<ParticleSystem>();

            _abilityPhaseService = new AbilityPhaseService(abilityApplicatorService, targetSelector);

            _controller.ParticleSystem1Started += OnParticleSystem1Started;
            _controller.ParticleSystem2Started += OnParticleSystem2Started;
            _controller.ParticleSystem3Started += OnParticleSystem3Started;

            _controller.Attack1Started += OnAttack;

            _controller.Finished += OnFinished;

            InitializeAnchors(_unit);
        }

        public void Dispose()
        {
            _controller.ParticleSystem1Started -= OnParticleSystem1Started;
            _controller.ParticleSystem2Started -= OnParticleSystem2Started;
            _controller.ParticleSystem3Started -= OnParticleSystem3Started;

            _controller.Attack1Started -= OnAttack;

            _controller.Finished -= OnFinished;
        }
        
        public void SetPhase(AbilityPhase phase)
        {
            _phase = phase;
        }

        private AbilityConfig SearchAbility() =>
            _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);

        private void OnParticleSystem1Started()
        {
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
            var a = _particleSystemFactory.Create(SearchAbility().EndAnimationParticles, _unit.transform,
                Quaternion.identity);
            
            _particles.AddRange(a);
        }

        private void OnAttack()
        {
            _abilityPhaseService.OnNext(_phase);
        }


        private void OnFinished()
        {
            foreach (var particle in _particles.ToList())
            {
                UnityEngine.Object.Destroy(particle.gameObject);
                _particles.Remove(particle);
            }
            
            ActionEnded?.Invoke();
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