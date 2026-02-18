using Effects;
using Services.StaticDataServices;
using Spawners;
using Units;

namespace Statuses
{
    public class StatusFactory : IStatusFactory
    {
        private readonly IEffectResolver _effectResolver;
        private readonly IStaticDataService _staticDataService;
        private readonly IParticleSpawner _particleSpawner;
        
        public StatusFactory(IEffectResolver effectResolver, IStaticDataService staticDataService, IParticleSpawner particleSpawner)
        {
            _effectResolver = effectResolver;
            _staticDataService = staticDataService;
            _particleSpawner = particleSpawner;
        }

        public Status Create(StatusSetup setup, Unit target) => 
            new( setup,  target,  _effectResolver, _staticDataService, _particleSpawner );
    }
}