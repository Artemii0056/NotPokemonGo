using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityView : MonoBehaviour
    {
        public float delta = 10f;
        
        private Unit _target;

        private Ability _ability;

        Rigidbody _rigidbody;
        
        private void Update()
        {
            if (_target == null)
                return;

            _rigidbody.velocity = Vector3.zero;
            
            transform.position =
                Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * delta);
        }

        public void Initialize(Unit targetUnit)
        {
            _target = targetUnit;
        }
    }
}