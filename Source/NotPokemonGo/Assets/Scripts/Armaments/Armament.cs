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

        public float delta = 5f;

        private List<EffectInfo> _effectInfo;
        private List<Status> _statuses;
        
        public Unit Source { get; private set; }
        public Unit Target { get; private set; }
        public List<EffectInfo> Effects =>  _effectInfo.ToList();
        public List<Status> Statuses => _statuses.ToList();

        public void Initialize(List<EffectInfo> effectInfo, List<Status> statuses, Unit source, Unit target)
        {
            _statuses = statuses;
            _effectInfo = effectInfo;
            Source = source;
            Target = target;
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