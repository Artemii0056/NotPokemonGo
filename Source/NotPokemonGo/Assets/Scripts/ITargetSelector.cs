using System.Collections.Generic;
using Abilities;
using Platoons;
using Units;

public interface ITargetSelector
{
    void Remember(Unit unit);
    List<Unit> GetTargets(TargetMode abilityModelTargetMode);
    void SetPlatoons(Platoon platoon, Platoon platoon2);
}