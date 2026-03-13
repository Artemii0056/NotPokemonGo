using System.Threading;
using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Presentation;
using Cysharp.Threading.Tasks;
using Spawners;

namespace AbilityNew.Scripts.Executors.Presentation
{
    public class SpawnVfxExecutor : AbilityStepExecutor<SpawnVfxStep>
    {
        private readonly IParticleSpawner _particleSpawner;

        public SpawnVfxExecutor(IParticleSpawner particleSpawner) => 
            _particleSpawner = particleSpawner;

        public override UniTask Execute(SpawnVfxStep step, AbilityExecutionRuntime runtime, CancellationToken ct)
        {
            _particleSpawner.Spawn(runtime.Context.Source, step.ParticleSpawnType, step.ParticleSystem);
            
            return UniTask.CompletedTask;
        }
    }
}