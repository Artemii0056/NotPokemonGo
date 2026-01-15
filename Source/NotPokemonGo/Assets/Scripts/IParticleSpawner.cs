using Units;
using Units.AnimationControllers;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IParticleSpawner
    {
        void Spawn(Unit owner, ParticleSpawnType spawnType, ParticleSystem prefab);
        void Clear(Unit owner);
    }
}