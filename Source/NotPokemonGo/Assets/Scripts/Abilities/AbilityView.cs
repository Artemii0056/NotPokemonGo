using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityView : MonoBehaviour
    {
        public float delta = 10f;
        
        private Unit _target;
        private Ability _ability;
        private EffectManager _effectManager;

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.TryGetComponent(out Unit unit))
        //     {
        //         if (_ability.HasStatus)
        //         {
        //             foreach (var status in _ability.Statuses) 
        //                 _effectManager.RegisterStatusEffect(status);
        //         }
        //
        //         if (_ability.HasEffect)
        //         {
        //             foreach (var effect in _ability.EffectInfo) 
        //                 unit.ReceiveDamage(effect);
        //         }
        //     }
        // }

        private void Update()
        {
            if (_target == null)
                return;

            transform.position =
                Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * delta);
        }

        public void Initialize(Ability ability, Unit targetUnit, EffectManager effectManager)
        {
            _ability = ability;
            _target = targetUnit;
            _effectManager = effectManager;
        }
    }
}