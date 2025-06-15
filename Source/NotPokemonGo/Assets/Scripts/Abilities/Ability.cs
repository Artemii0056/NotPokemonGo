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

        private Transform _targetTransform;

        private List<Status> _statuses;
        private List<EffectInfo> _effectInfo;

        public Ability(AbilityConfig abilityConfig,List<Status> statuses, List<EffectInfo> effectInfo ,Unit target)
        {
            _targetTransform = target.transform;
            _cost = abilityConfig.Cost;

            _statuses = statuses;
            _effectInfo = effectInfo;
        }

        public TargetMode TargetMode { get; private set; }

        public bool HasStatus => _statuses.Count > 0;
        public bool HasEffect => _effectInfo.Count > 0;

        public List<Status> Statuses => _statuses.ToList();
        public List<EffectInfo> EffectInfo => _effectInfo.ToList();
    }
}