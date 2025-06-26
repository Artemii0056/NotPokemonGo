using Units;
using UnityEngine;

namespace Animations
{
    public class AbilityAnimationHook : MonoBehaviour
    {
        public Unit unit;
        
        public void OnHit()
        {
            unit.ApplyAnimationEvent();
        }
    }
}