using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.MV;
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
        private IAbilityApplicatorService _abilityApplicatorService;
        private ITargetSelector _targetSelector;
        
        private AbilityPhaseService _abilityPhaseService;

        private UnitAnimatorController _controller;
        private Unit _unit;

        private Dictionary<AbilityType, AbilityAnchor> _anchors;
        private  List<ParticleSystem> _particles;

        public event Action ActionEnded;
        
        private List<EffectSetup> _effects;

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
            _abilityApplicatorService = abilityApplicatorService;
            _targetSelector = targetSelector;
            _effects = new List<EffectSetup>();
            _particles = new List<ParticleSystem>();

            _abilityPhaseService = new AbilityPhaseService(abilityApplicatorService, targetSelector);
            _abilityPhaseService.Initialize(_abilityProvider.AbilityModel);

            _controller.ParticleSystem1Started += OnParticleSystem1Started;
            _controller.ParticleSystem2Started += OnParticleSystem2Started;
            _controller.ParticleSystem3Started += OnParticleSystem3Started;

            _controller.Attack1Started += OnAttack1Started;
            _controller.Attack2Started += OnAttack2Started;

            _controller.Finished += OnFinished;

            InitializeAnchors(_unit);
        }

        public void Dispose()
        {
            _controller.ParticleSystem1Started -= OnParticleSystem1Started;
            _controller.ParticleSystem2Started -= OnParticleSystem2Started;
            _controller.ParticleSystem3Started -= OnParticleSystem3Started;

            _controller.Attack1Started -= OnAttack1Started;
            _controller.Attack2Started -= OnAttack2Started;

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

        private void OnAttack1Started()
        {
            _abilityPhaseService.Initialize(_abilityProvider.AbilityModel);
            
            // AbilityModel ability = _abilityProvider.AbilityModel; // Todo - разделить логику? Передавать и абилку/список абилок?
            //
            // if (ability.CastamentSetup.EffectsSetup.Count > 1)
            // {
            //     _effects = ability.CastamentSetup.EffectsSetup;
            // }
            
            Debug.Log("OnAttack1Started");
            
            _abilityPhaseService.OnNextTrigger();
            
            // _abilityApplicatorService
            //     .Apply(_targetSelector.GetTargets(ability.TargetMode)
            //         .ToArray());
        }
        
        private void OnAttack2Started()
        {
            Debug.Log("OnAttack2Started");
            
            _abilityPhaseService.OnNextTrigger();

            // _abilityApplicatorService
            //     .Apply(_targetSelector.GetTargets(_abilityProvider.AbilityModel.TargetMode)
            //         .ToArray());
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