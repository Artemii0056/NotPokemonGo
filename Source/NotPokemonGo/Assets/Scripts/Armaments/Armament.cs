using System.Collections.Generic;
using System.Linq;
using Effects;
using Statuses;
using UnityEngine;
using Unit = Units.Unit;

namespace Armaments
{
    public class Armament : MonoBehaviour
    {
        [field: SerializeField] public ParticleSystem _particleSystemPrefab;

        private List<EffectInfo> _effectInfo;
        private List<Status> _statuses;

        public Unit Source { get; private set; }
        public Unit Target { get; private set; }
        public List<EffectInfo> Effects =>  _effectInfo.ToList();
        public List<Status> Statuses => _statuses.ToList();
        public ArmamentFlyingType FlyingType { get; private set; } = ArmamentFlyingType.Direct;
        public ArmamentSetup Setup { get;  private set; }

        public void Initialize(List<EffectInfo> effectInfo, List<Status> statuses, Unit source, Unit target, ArmamentSetup setup, ArmamentFlyingType flyingType)
        {
            _statuses = statuses;
            _effectInfo = effectInfo;
            Source = source;
            Target = target;
            Setup = setup;
            FlyingType =flyingType;
        }

        private void Start()
        {
            if (_particleSystemPrefab == null)
                return;
            
            var parcticle = Instantiate(_particleSystemPrefab, transform);
            parcticle.Play();
        }
    }
}