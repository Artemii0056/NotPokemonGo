using System.Collections.Generic;
using UnityEngine;

namespace Units.AnimationControllers
{
    public class ParticleSystemFactory : IParticleSystemFactory //TODO Не нужен больше? 
    {
        public List<ParticleSystem> Create(List<ParticleSystem> particleSystems, Transform transform, Quaternion rotation)
        {
            List<ParticleSystem> particleSystemList = new List<ParticleSystem>();
            
            foreach (ParticleSystem particleSystem in particleSystems)
            {
              ParticleSystem system =  Object.Instantiate(particleSystem, transform.position, Quaternion.identity, transform);
              particleSystemList.Add(system);
              system.Play();
            }

            return particleSystemList;
        }
    }
}