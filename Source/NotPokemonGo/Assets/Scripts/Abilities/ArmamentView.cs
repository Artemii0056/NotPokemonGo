using UnityEngine;
using UnityEngine.Serialization;
using Unit = Units.Unit;

namespace Abilities
{
    public class ArmamentView : MonoBehaviour
    {
        [FormerlySerializedAs("_particleSystem")] [SerializeField] private ParticleSystem _particleSystemPrefab;

        public float delta = 10f;

        private Unit _target;

        private void Start()
        {
            var parcticle = Instantiate(_particleSystemPrefab, transform);
            parcticle.Play();
        }

        private void Update()
        {
            if (_target == null)
                return;

            transform.position =
                Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * delta);
        }

        public void Initialize(Unit targetUnit)
        {
            _target = targetUnit;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit))
            {
                if (_target == unit)
                {
                    // Destroy(gameObject);
                }
            }
        }
    }
}