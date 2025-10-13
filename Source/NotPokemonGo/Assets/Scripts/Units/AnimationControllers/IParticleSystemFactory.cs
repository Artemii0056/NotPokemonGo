using System.Collections.Generic;
using UnityEngine;

namespace Units.AnimationControllers
{
    public interface IParticleSystemFactory
    {
        List<ParticleSystem> Create(List<ParticleSystem> particleSystems, Transform transform, Quaternion rotation);
    }
}