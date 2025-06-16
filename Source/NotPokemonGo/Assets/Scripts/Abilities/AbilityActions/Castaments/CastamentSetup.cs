using System;
using System.Collections.Generic;
using Effects;
using Statuses;
using UnityEngine;

namespace Abilities.AbilityActions.Castaments
{
    [Serializable]
    public class CastamentSetup
    {
        [field: SerializeField] public List<StatusSetup> Statuses { get; private set; }
        [field: SerializeField] public List<EffectSetup> EffectInfo { get; private set; }
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
    }
}