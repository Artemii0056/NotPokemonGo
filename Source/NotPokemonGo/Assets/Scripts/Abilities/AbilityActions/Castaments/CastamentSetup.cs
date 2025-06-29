using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public List<StatusSetup> StatusesSetup => Statuses.ToList();
        public List<EffectSetup> EffectsSetup => EffectInfo.ToList();
        
        public bool HasSetupData => StatusesSetup.Any() || EffectsSetup.Any();
        //[field: SerializeField] public CastamentView ArmamentView { get; private set; }
    }
}