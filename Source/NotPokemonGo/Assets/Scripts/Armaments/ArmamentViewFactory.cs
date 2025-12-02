using System.Collections.Generic;
using Effects;
using Statuses;
using Units;
using UnityEngine;

namespace Armaments
{
    public class ArmamentViewFactory : IArmamentViewFactory
    {
        public Armament Create(
            List<EffectInfo> effects, 
            List<Status> statuses, 
            Vector3 position, 
            Armament prefab,
            Unit source,
            Unit target)
        {
            Armament armament = Object.Instantiate(prefab, position, Quaternion.identity);
            armament.Initialize(effects,  statuses, source, target);
            Debug.LogError("created");
            return armament;
        }
    }
}