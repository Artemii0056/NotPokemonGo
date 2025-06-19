using System.Collections.Generic;
using System.Linq;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using Statuses;
using Units;
using UnityEngine;

namespace Abilities
{
    public class Ability
    {
        private List< Transform> _targetTransform;

        public ArmamentSetup ArmamentSetup { get; private set; }
        public CastamentSetup CastamentSetup { get; private set; }

        public Ability(AbilityConfig abilityConfig, params Unit[] targets)
        {
            _targetTransform = new List<Transform>();
            
            foreach (Unit unit in targets) 
                _targetTransform.Add(unit.transform);

            ArmamentSetup = abilityConfig.ArmamentSetup;
            CastamentSetup = abilityConfig.CastamentSetup;
        }
        
        public bool HasArmament => ArmamentSetup != null;
        public bool HasCastament => CastamentSetup != null;

        public TargetMode TargetMode { get; private set; }
    }
}