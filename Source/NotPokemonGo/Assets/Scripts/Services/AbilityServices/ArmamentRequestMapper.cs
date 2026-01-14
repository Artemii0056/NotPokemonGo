using System.Collections.Generic;
using Armaments;

namespace Services.AbilityServices
{
    public static class ArmamentRequestMapper
    {
        public static IEnumerable<ArmamentContext> ToContextsPerTarget(ArmamentRequest req)
        {
            var setup = req.Setup;
            var source = req.Source;

            if (source == null || setup == null || req.Targets == null)
                yield break;

            foreach (var t in req.Targets)
            {
                if (t == null) 
                    continue;
                
                yield return new ArmamentContext(source, t, setup, setup.FlyingType);
            }
        }

        public static ArmamentContext? ToSingleContextFirstTarget(in ArmamentRequest req)
        {
            if (req.Targets == null || req.Targets.Length == 0 || req.Targets[0] == null)
                return null;

            return new ArmamentContext(req.Source, req.Targets[0], req.Setup, req.Setup.FlyingType);
        }
    }
}