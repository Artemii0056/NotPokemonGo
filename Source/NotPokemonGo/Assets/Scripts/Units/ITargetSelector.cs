using System.Collections.Generic;
using Abilities;
using Platoons;

namespace Units
{
    public interface ITargetSelector
    {
        List<Unit> GetTargets(TargetMode abilityModelTargetMode, Unit target);
        void SetPlatoons(Platoon platoon, Platoon platoon2);
    }
}