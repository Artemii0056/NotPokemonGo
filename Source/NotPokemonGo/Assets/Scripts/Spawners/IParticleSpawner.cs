using Units;
using UnityEngine;

namespace Spawners
{
    public interface IParticleSpawner
    {
        void Spawn(Unit owner, ParticleSpawnType spawnType, ParticleSystem prefab);
        void Clear(Unit owner);
    }
}