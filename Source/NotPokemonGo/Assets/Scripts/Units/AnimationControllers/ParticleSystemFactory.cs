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
        
        public void Create(List<ParticleSystem> particleSystems, Vector3 position, Quaternion rotation)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
              var a =  GameObject.Instantiate(particleSystem,  position, rotation);
              a.Play();
            }
        }
    }
}