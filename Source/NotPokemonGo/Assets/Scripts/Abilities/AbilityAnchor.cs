using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class AbilityAnchor
    {
        public ParticleSpawnType spawnType;
        public List<Transform> Transforms;
    }
}