using Units;
using UnityEngine;

namespace Spawners
{
    public interface IParticleSpawner
    {
        void Spawn(Unit owner, ParticleSpawnType spawnType, ParticleSystem prefab);
        void Spawn(Unit target, ParticleSystem prefab);
        void Spawn(Transform point, ParticleSystem prefab);
        void Clear(Unit owner);
    }
}