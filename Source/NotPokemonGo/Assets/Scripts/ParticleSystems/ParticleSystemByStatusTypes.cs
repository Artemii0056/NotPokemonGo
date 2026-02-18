using System.Collections.Generic;
using UnityEngine;

namespace ParticleSystems
{
    [CreateAssetMenu(fileName = nameof(ParticleSystemByStatusTypes), menuName = "StaticData/" + nameof(ParticleSystemByStatusTypes))]
    public class ParticleSystemByStatusTypes : ScriptableObject
    {
        public List<SystemByStatusType> PrticleSystemByStatusType = new List<SystemByStatusType>();
    }
}