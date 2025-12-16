using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.Configs;
using Armaments;
using Castaments;
using ReactionSystems;
using Services.AbilityServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Units.AnimationControllers
{
    public class UnitAnimatorTrigger : IDisposable
    {
        private AbilityPhaseService _abilityPhaseService;

        private UnitAnimatorController _controller;
        private Unit _unit;

        public Dictionary<ParticleSpawnType, AbilityAnchor> _anchors; //TODO public
        private List<ParticleSystem> _particles;

        private AbilityPhase _phase;

        private Unit _currentTarget;

        public UnitAnimatorTrigger
        (
            Unit unit,
            UnitAnimatorController controller,
            ICastamentApplicator castamentApplicator,
            IArmamentApplicator armamentApplicator,
            ITargetSelector targetSelector,
            IReactionService reactionService
            )
        {
            _unit = unit;
            _controller = controller;

            _particles = new List<ParticleSystem>();

            _abilityPhaseService =
                new AbilityPhaseService(castamentApplicator, armamentApplicator, targetSelector, reactionService);

            _controller.ParticleSystem1Started += OnParticleSystem1Started; //Отдельный класс с реакцией на партикды
            _controller.ParticleSystem2Started += OnParticleSystem2Started;

            _controller.Attack1Started += OnAttack;

            InitializeAnchors(_unit);

            _unit.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float arg1, float arg2)
        {
            if (_unit.AbilityHandler == null)
                return;

            if (_unit.AbilityHandler.Interruptibility == Interruptibility.CannotBeInterrupted)
                return;

            Debug.Log("TakeDamage");
            _controller.Play(Constants.BaseAnimations.TakeDamage);
        }

        public void Dispose()
        {
            _controller.ParticleSystem1Started -= OnParticleSystem1Started;
            _controller.ParticleSystem2Started -= OnParticleSystem2Started;

            _controller.Attack1Started -= OnAttack;

            _unit.HealthChanged -= OnHealthChanged;
        }

        public void SetTarget(Unit target)
        {
            _currentTarget = target;
        }

        public void SetPhase(AbilityPhase phase)
        {
            _phase = phase;
        }

        private void OnParticleSystem1Started()
        {
            var info = _phase.ParticleSystemBySpawnType[0];

            ParticleSystem system = info.ParticleSystem;

            ParticleSpawnType type = info.ParticleSpawnType;

            if (_anchors.TryGetValue(type, out AbilityAnchor anchor))
            {
                Debug.Log(anchor.spawnType);
                
                Transform point = anchor.Transforms[0];

                ParticleSystem particleSystemPrefab =
                    Object.Instantiate(system, point.position, Quaternion.identity, point);
                
                particleSystemPrefab.Play();
                _particles.Add(particleSystemPrefab);
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
                var point = anchor.Transforms[0];

                var ps = Object.Instantiate(system, point.position, Quaternion.identity, point);
                ps.Play();
            }
        }

        private void OnAttack()
        {
            _abilityPhaseService.OnNext(_phase, _unit, _currentTarget);
        }

        public void ClearParticles()
        {
            foreach (var particle in _particles.ToList())
            {
                Object.Destroy(particle.gameObject);
                _particles.Remove(particle);
            }
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