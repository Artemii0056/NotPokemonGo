using UnityEngine;
using Unit = Units.Unit;

namespace Abilities
{
    public class ArmamentView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public float delta = 10f;

        private Unit _target;

        private void Start()
        {
            _particleSystem.Play();
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
                  Destroy(gameObject);
                }
            }
        }
    }
}