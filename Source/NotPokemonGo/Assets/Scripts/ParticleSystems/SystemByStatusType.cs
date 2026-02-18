using System;
using Statuses;
using UnityEngine;

namespace ParticleSystems
{
    [Serializable]
    public class SystemByStatusType
    {
        [SerializeField] public StatusType StatusType;
        [SerializeField] public ParticleSystem ParticleSystem;
    }
}