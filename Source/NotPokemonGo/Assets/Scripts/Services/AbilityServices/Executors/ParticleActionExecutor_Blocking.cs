using Abilities.Configs;
using Abilities.Runtime;
using DG.Tweening;
using Services.AbilityServices;
using Spawners;
using Units;
using UnityEngine;

namespace Services.AbilityServices.Executors
{
    /// <summary>
    /// Variant B: particles BLOCK phase completion until their expected lifetime ends.
    /// Use for: gameplay-critical VFX where you want phase to wait.
    /// 
    /// NOTE: This uses prefab duration estimation (main.duration + startLifetime.max).
    /// If you use looping systems, DO NOT use this variant (it will wait forever).
    /// </summary>
    public sealed class ParticleActionExecutor_Blocking : IPhaseSignalActionExecutor
    {
        private readonly IParticleSpawner _particleSpawner;

        public ParticleActionExecutor_Blocking(IParticleSpawner particleSpawner) =>
            _particleSpawner = particleSpawner;

        public bool CanExecute(PhaseSignalAction action) =>
            action != null && action.HasParticle;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate)
        {
            if (_particleSpawner == null || action == null)
                return;

            Unit owner = action.ParticleOwner == ParticleOwner.Source ? source : target;
            if (owner == null || action.ParticlePrefab == null)
                return;

            _particleSpawner.Spawn(owner, action.ParticlePrefab);

            float waitSeconds = EstimateLifetimeSeconds(action.ParticlePrefab);
            if (waitSeconds <= 0f)
                return;

            string tag = $"ParticleWait phase={phase.AnimationCashName} dur={waitSeconds:0.###} src={(source != null ? source.Id : -1)}";

            float dummy = 0f;
            Tween timer = DOTween.To(() => dummy, _ => { }, 1f, waitSeconds)
                .SetEase(Ease.Linear);

            // Holds phase gate until timer ends; is killed on ability Stop via scope ownership.
            timer
                .WithPhaseGate(finishGate, tag)
                .OwnedByGate(finishGate);
        }

        private static float EstimateLifetimeSeconds(ParticleSystem prefab)
        {
            if (prefab == null)
                return 0f;

            var main = prefab.main;

            // Looping particles must not block.
            if (main.loop)
                return 0f;

            float duration = Mathf.Max(0f, main.duration);
            float lifetimeMax = 0f;

            var lifetime = main.startLifetime;
            switch (lifetime.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    lifetimeMax = lifetime.constant;
                    break;
                case ParticleSystemCurveMode.TwoConstants:
                    lifetimeMax = lifetime.constantMax;
                    break;
                case ParticleSystemCurveMode.Curve:
                    lifetimeMax = lifetime.curve != null ? lifetime.curve.keys[^1].time : 0f;
                    break;
                case ParticleSystemCurveMode.TwoCurves:
                    lifetimeMax = lifetime.curveMax != null ? lifetime.curveMax.keys[^1].time : 0f;
                    break;
            }

            return duration + Mathf.Max(0f, lifetimeMax);
        }
    }
}
