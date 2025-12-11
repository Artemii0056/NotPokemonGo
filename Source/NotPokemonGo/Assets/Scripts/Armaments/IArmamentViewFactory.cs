using System.Collections.Generic;
using Effects;
using Statuses;
using Units;
using UnityEngine;

namespace Armaments
{
    public interface IArmamentViewFactory
    {
        Armament Create(
            List<EffectInfo> effects, 
            List<Status> statuses, 
            Vector3 position, 
            Armament prefab,
            Unit source,
            Unit target,
            ArmamentSetup setup);
    }
}