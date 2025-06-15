using System.Collections.Generic;
using System.Linq;
using Statuses;
using Units;
using UnityEngine;

namespace Abilities
{
    public class Ability
    {
        private float _cost;

        private Sprite _icon;

        private Transform _target;
        
        private List<Status>  _statuses;
        private List<EffectInfo> _effectInfo;
        
        public Ability(AbilityConfig abilityConfig, Unit target, Unit source, EffectResolver effectResolver)
        {
            _target = target.transform;
            _cost = abilityConfig.Cost;
            
            _statuses = new List<Status>();
            _effectInfo = new List<EffectInfo>();

            foreach (var statusSetup in abilityConfig.Statuses) 
                _statuses.Add(new HealStatus(statusSetup, target, source, effectResolver)); //TODO тут нужна фабрика по созданию эффектов

            foreach (var effectInfo in abilityConfig.EffectInfo) 
                _effectInfo.Add(new EffectInfo(effectInfo.Type,effectInfo.Value, source ));
        }
        
        public TargetMode TargetMode { get; private set; }
        
        public bool HasStatus => _statuses.Count > 0;
        public bool HasEffect => _effectInfo.Count > 0;
        
        public List<Status> Statuses => _statuses.ToList();
        public List<EffectInfo> EffectInfo => _effectInfo.ToList();
    }
}