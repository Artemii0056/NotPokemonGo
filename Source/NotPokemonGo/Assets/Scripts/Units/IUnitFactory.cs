using System.Collections.Generic;
using Characters;
using Platoons;
using Stats;
using UnityEngine;

namespace Units
{
    public interface IUnitFactory
    {
        Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType);

        Unit Create(Vector3 spawnPosition, Transform parentPosition, UnitConfig config, PlatoonType platoonType,
            Dictionary<StatType, StatSetup> stats);
    }
}