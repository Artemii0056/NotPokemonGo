using System.Collections.Generic;
using Services.StaticDataServices;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class ParticleSystemFactory : IParticleSystemFactory
    {
        private readonly IStaticDataService _staticDataService;

        public ParticleSystemFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public List<ParticleSystem> Create(List<ParticleSystem> particleSystems, Transform transform, Quaternion rotation)
        {
            List<ParticleSystem> particleSystemList = new List<ParticleSystem>();
            
            foreach (ParticleSystem particleSystem in particleSystems)
            {
              //ParticleSystem system =  GameObject.Instantiate(particleSystem,  position, rotation);
              ParticleSystem system =  GameObject.Instantiate(particleSystem, transform.position, Quaternion.identity, transform);
              particleSystemList.Add(system);
              system.Play();
            }

            return particleSystemList;
        }
    }
}