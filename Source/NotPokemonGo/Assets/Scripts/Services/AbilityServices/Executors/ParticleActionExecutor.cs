using System;
using Abilities.Configs;
using Spawners;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class ParticleActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly IParticleSpawner _particleSpawner;

        public ParticleActionExecutor(IParticleSpawner particleSpawner) => 
            _particleSpawner = particleSpawner;

        public bool CanExecute(PhaseSignalAction action) => 
            action != null && action.HasParticle;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate, Action tryCompleteFinish)
        {
            if (action.ParticlePrefab == null)
                return;

            var owner = action.ParticleOwner == ParticleOwner.Source ? source : target;
            
            if (owner == null)
                return;

            _particleSpawner.Spawn(owner, action.ParticleSpawnType, action.ParticlePrefab);
            return;
        }
    }
}
