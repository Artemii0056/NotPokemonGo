using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.Configs;
using Assets;
using ReactionSystems;
using Services.AbilityServices;
using Services.StaticDataServices;
using UnityEngine;
using Object = UnityEngine.Object;

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

        private Dictionary<ParticleSpawnType, AbilityAnchor> _anchors;
        private List<ParticleSystem> _particles;

        private AbilityPhase _phase;

        private Unit _currentTarget;

        public event Action ActionEnded;

        public UnitAnimatorTrigger(
            Unit unit,
            IStaticDataService staticDataService,
            IAbilityProvider abilityProvider,
            UnitAnimatorController controller,
            IParticleSystemFactory particleSystemFactory,
            IAbilityApplicatorService abilityApplicatorService,
            ITargetSelector targetSelector,
            IReactionService reactionService)
        {
            _unit = unit;
            _staticDataService = staticDataService;
            _abilityProvider = abilityProvider;
            _controller = controller;
            _particleSystemFactory = particleSystemFactory;

            _particles = new List<ParticleSystem>();

            _abilityPhaseService = new AbilityPhaseService(abilityApplicatorService, targetSelector, reactionService);

            _controller.ParticleSystem1Started += OnParticleSystem1Started;
            _controller.ParticleSystem2Started += OnParticleSystem2Started;

            _controller.Attack1Started += OnAttack;

            _controller.Finished += OnFinished;

            InitializeAnchors(_unit);
        }

        public void Dispose()
        {
            _controller.ParticleSystem1Started -= OnParticleSystem1Started;
            _controller.ParticleSystem2Started -= OnParticleSystem2Started;

            _controller.Attack1Started -= OnAttack;

            _controller.Finished -= OnFinished;
        }

        public void SetTarget(Unit target)
        {
            _currentTarget = target;
        }

        public void SetPhase(AbilityPhase phase)
        {
            _phase = phase;
        }

        private AbilityConfig SearchAbility() => //TODO
            _staticDataService.GetAbilityConfig(_abilityProvider.AbilityModel.AbilityType);

        private void OnParticleSystem1Started()
        {
            var info = _phase.ParticleSystemBySpawnType[0];

            ParticleSystem system = info.ParticleSystem;

            ParticleSpawnType type = info.ParticleSpawnType;
            Debug.Log(type);

            if (_anchors.TryGetValue(type, out AbilityAnchor anchor))
            {
                Transform point = anchor.Transforms[0];

                ParticleSystem particleSystem = Object.Instantiate(system, point.position, Quaternion.identity, point);
                particleSystem.Play();
                _particles.Add(particleSystem);
            }
        }

        private void OnParticleSystem2Started()
        {
            var info = _phase.ParticleSystemBySpawnType[0];

            ParticleSystem system = info.ParticleSystem;

            ParticleSpawnType type = info.ParticleSpawnType;
            Debug.Log(type);

            if (_anchors.TryGetValue(type, out AbilityAnchor anchor))
            {
                Debug.Log("In On");

                var point = anchor.Transforms[0];

                var ps = Object.Instantiate(system, point.position, Quaternion.identity, point);
                ps.Play();
                //_particles.AddRange(a);
            }
        }

        private void OnAttack()
        {
            _abilityPhaseService.OnNext(_phase, _unit, _currentTarget);
        }

        private void OnFinished()
        {
            foreach (var particle in _particles.ToList())
            {
                Object.Destroy(particle.gameObject);
                _particles.Remove(particle);
            }

            ActionEnded?.Invoke();
        }

        private void InitializeAnchors(Unit unit)
        {
            _anchors = new();

            foreach (var anchor in unit.AbilityAnchors)
            {
                if (!_anchors.TryAdd(anchor.spawnType, anchor))
                {
                    Debug.LogWarning($"Дубликат Anchor для {anchor.spawnType} у {unit.name}");
                }
            }
        }
    }
}