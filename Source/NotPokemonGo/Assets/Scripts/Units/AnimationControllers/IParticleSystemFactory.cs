using System.Collections.Generic;
using UnityEngine;

namespace Units.AnimationControllers
{
    public interface IParticleSystemFactory
    {
        void Create(List<ParticleSystem> particleSystems, Vector3 position, Quaternion rotation);
    }
}