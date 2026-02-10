using System.Collections.Generic;
using Armaments;
using Units;
using UnityEngine;

namespace Services.AbilityServices
{
    public static class ArmamentRequestMapper
    {
        public static IEnumerable<ArmamentContext> EnumerateContexts(ArmamentRequest req, Transform transform = null)
        {
            Transform spawnPosition = transform;
            
            if (transform == null) 
                spawnPosition = req.Source.abilityPos;
            
            if (req.Source == null) 
                yield break;

            ArmamentSetup setup = req.Setup;
            
            if (setup == null || !setup.HasSetupData) 
                yield break;

            Unit[] targets = req.Targets;
            
            if (targets == null || targets.Length == 0) 
                yield break;

            for (int i = 0; i < targets.Length; i++)
            {
                Unit target = targets[i];
                
                if (target == null) 
                    continue;

                yield return new ArmamentContext(req.Source, target, setup, setup.FlyingType,spawnPosition);
            }
        }
    }
}