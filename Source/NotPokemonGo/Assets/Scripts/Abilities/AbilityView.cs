using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityView : MonoBehaviour
    {
        public Unit target;
        public float delta = 10f;
        public Ability _ability;
        public EffectManager effectManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit))
            {
                if (_ability.HasStatus)
                {
                    foreach (var status in _ability.Statuses) 
                        effectManager.RegisterStatusEffect(status);
                }

                if (_ability.HasEffect)
                {
                    foreach (var effect in _ability.EffectInfo)
                    {
                        unit.ReceiveDamage(effect);
                    }
                }
            }
        }

        private void Update()
        {
            if (target == null)
                return;

            transform.position =
                Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * delta);
        }
    }
}