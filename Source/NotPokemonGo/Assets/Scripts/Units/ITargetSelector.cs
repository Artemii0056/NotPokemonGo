using System.Collections.Generic;
using Abilities;
using Platoons;

namespace Units
{
    public interface ITargetSelector
    {
        IReadOnlyList<Unit> GetTargets(TargetMode targetMode, Unit source, Unit primaryTarget);
        void SetPlatoons(Platoon platoon, Platoon platoon2);
        Unit GetRandomEnemyTarget();
    }
}