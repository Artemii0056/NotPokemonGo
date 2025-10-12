using System.Collections.Generic;
using Characters;
using Stats;
using Units;
using UnityEngine;

namespace Factories
{
    public interface IUnitFactory
    {
        Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType);

        Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType,
            Dictionary<StatType, StatSetup> stats);
    }
}