using Abilities.Configs;
using Spawners;
using Units;

namespace Services.AbilityServices.Executors
{
    /// <summary>
    /// Variant A: particles do NOT block phase completion.
    /// Recommended for cosmetic VFX.
    /// </summary>
    public sealed class ParticleActionExecutor_NonBlocking : IPhaseSignalActionExecutor
    {
        private readonly IParticleSpawner _particleSpawner;

        public ParticleActionExecutor_NonBlocking(IParticleSpawner particleSpawner) =>
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

            // Ownership is intentionally NOT applied here: particles are usually pooled/managed by spawner.
            _particleSpawner.Spawn(owner, action.ParticlePrefab);
        }
    }
}
