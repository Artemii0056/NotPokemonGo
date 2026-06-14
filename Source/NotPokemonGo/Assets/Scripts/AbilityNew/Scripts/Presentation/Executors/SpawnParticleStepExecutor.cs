using Spawners;

namespace AbilityNew.Scripts.Presentation.Executors
{
    public sealed class SpawnParticleStepExecutor : PresentationStepExecutor<SpawnParticleStep>
    {
        private readonly IParticleSpawner _particleSpawner;

        public SpawnParticleStepExecutor(IParticleSpawner particleSpawner)
        {
            _particleSpawner = particleSpawner;
        }

        protected override void Execute(SpawnParticleStep step, AbilityPresentationContext context)
        {
            if (step.Prefab == null)
                return;

            switch (step.Anchor)
            {
                case PresentationAnchor.Caster:
                    if (context.Caster != null)
                        _particleSpawner.Spawn(context.Caster, step.SpawnType, step.Prefab);
                    break;

                case PresentationAnchor.Target:
                    if (context.Target != null)
                        _particleSpawner.Spawn(context.Target, step.Prefab);
                    break;

                case PresentationAnchor.ExplicitTransform:
                    if (context.ExplicitTransform != null) 
                        _particleSpawner.Spawn(context.ExplicitTransform.transform, step.Prefab);
                    break;
            }
        }
    }
}