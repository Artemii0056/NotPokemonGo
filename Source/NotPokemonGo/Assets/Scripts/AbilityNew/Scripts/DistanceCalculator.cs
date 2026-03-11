using Units;
using UnityEngine;

namespace AbilityNew.Scripts
{
    public class DistanceCalculator
    {
        public Vector3 CalculateDistance(Unit source, Unit target)
        {
            if (target == null) 
                return source.transform.position;
                    
            Vector3 from = source.transform.position;
            Vector3 to = target.transform.position;
            Vector3 dir = to - from;
                    
            if (dir.sqrMagnitude < 0.0001f) 
                return from;
                    
            dir.Normalize();
            
            return to - dir ;
        }
    }
}