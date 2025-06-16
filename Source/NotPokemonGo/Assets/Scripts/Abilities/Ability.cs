using System.Collections.Generic;
using System.Linq;
using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;
using Statuses;
using Units;
using UnityEngine;
using UnityEngine.UIElements;

namespace Abilities
{
    public class Ability
    {
        private float _cost;

        private Sprite _icon;

        private List< Transform> _targetTransform;

        // private List<Status> _statuses;
        // private List<EffectInfo> _effectInfo;

        public ArmamentSetup ArmamentSetup { get; private set; }
        public CastamentSetup CastamentSetup { get; private set; }

        public Ability(AbilityConfig abilityConfig, params Unit[] targets)
        {
            _targetTransform = new List<Transform>();
            
            foreach (Unit unit in targets) 
                _targetTransform.Add(unit.transform);

            _cost = abilityConfig.Cost;
            
            ArmamentSetup = abilityConfig.ArmamentSetup;
            CastamentSetup = abilityConfig.CastamentSetup;

            // _statuses = statuses;
            // _effectInfo = effectInfo;
        }
        
        public bool HasArmament => ArmamentSetup != null;

        public TargetMode TargetMode { get; private set; }

        // public bool HasStatus => _statuses.Count > 0;
        // public bool HasEffect => _effectInfo.Count > 0;
        //
        // public List<Status> Statuses => _statuses.ToList();
        // public List<EffectInfo> EffectInfo => _effectInfo.ToList();
    }
}