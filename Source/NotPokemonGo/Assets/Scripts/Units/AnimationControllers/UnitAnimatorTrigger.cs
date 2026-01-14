using System;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using Abilities.Configs;
using Abilities.Signals;
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
        public AbilityPhaseService AbilityPhaseService { get; }

        private AnimatorController _controller;
        private Unit _unit;

        public Dictionary<ParticleSpawnType, AbilityAnchor> _anchors; //TODO public
        private List<ParticleSystem> _particles;

        private AbilityPhase _phase;

        private Unit _currentTarget;

        public UnitAnimatorTrigger
        (
            Unit unit,
            AnimatorController controller,
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IReactionService reactionService
            )
        {
            _unit = unit;
            _controller = controller;

            _controller.Signal += OnSignal;

            _particles = new List<ParticleSystem>();

            AbilityPhaseService =
                new AbilityPhaseService(castamentApplicator, targetSelector, reactionService);

            _controller.Particle1 += OnParticle1; //Отдельный класс с реакцией на партикды
            _controller.Particle2 += OnParticle2;

            _controller.Attack1 += OnAttack;

            InitializeAnchors(_unit);

            _unit.HealthChanged += OnHealthChanged;
        }

        private void OnSignal(PhaseSignal signal) => 
            AbilityPhaseService.OnSignal(_phase, signal, _unit, _currentTarget);

        private void OnHealthChanged(float arg1, float arg2)
        {
            Debug.Log("OnHealthChanged");
            
            if (_unit.AbilityHandler == null)
                return;

            if (_unit.AbilityHandler.Interruptibility == Interruptibility.CannotBeInterrupted)
                return;

            Debug.Log("TakeDamage");
            _controller.Play(Constants.BaseAnimations.TakeDamage);
        }

        public void Dispose()
        {
            _controller.Particle1 -= OnParticle1;
            _controller.Particle2 -= OnParticle2;

            _controller.Attack1 -= OnAttack;

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

        private void OnParticle1()
        {
            var info = _phase.ParticleSystemBySpawnType[0];

            ParticleSystem system = info.ParticleSystem;

            ParticleSpawnType type = info.ParticleSpawnType;

            if (_anchors.TryGetValue(type, out AbilityAnchor anchor))
            {
                Transform point = anchor.Transforms[0];

                ParticleSystem particleSystemPrefab =
                    Object.Instantiate(system, point.position, Quaternion.identity, point);
                
                particleSystemPrefab.Play();
                _particles.Add(particleSystemPrefab);
            }
        }

        private void OnParticle2()
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
            AbilityPhaseService.OnNext(_phase, _unit, _currentTarget);
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