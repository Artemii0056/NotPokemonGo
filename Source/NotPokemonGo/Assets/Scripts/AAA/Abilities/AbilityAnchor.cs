using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abilities
{
    [Serializable]
    public class AbilityAnchor
    {
        [FormerlySerializedAs("spawnType")] public ParticleSpawnType SpawnType;
        [FormerlySerializedAs("Transforms")] public Transform Transform;
    }
}