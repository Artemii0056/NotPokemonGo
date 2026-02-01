using System.Collections.Generic;
using Armaments;
using Units;

namespace Services.AbilityServices
{
    public static class ArmamentRequestMapper
    {
        public static IEnumerable<ArmamentContext> EnumerateContexts(ArmamentRequest req)
        {
            if (req.Source == null) yield break;

            var setup = req.Setup;
            if (setup == null || !setup.HasSetupData) yield break;

            var targets = req.Targets;
            if (targets == null || targets.Length == 0) yield break;

            for (int i = 0; i < targets.Length; i++)
            {
                Unit t = targets[i];
                if (t == null) continue;

                yield return new ArmamentContext(req.Source, t, setup, setup.FlyingType);
            }
        }
    }
}