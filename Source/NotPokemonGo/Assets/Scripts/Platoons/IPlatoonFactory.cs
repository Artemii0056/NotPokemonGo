using System.Collections.Generic;
using Characters;
using UI.SpawnPositions;
using Units;
using UnityEngine;

namespace Platoons
{
    public interface IPlatoonFactory
    {
        Platoon Create(
            PlatoonSpawnContainer container,
            Transform platoonPosition,
            PlatoonType platoonType,
            UnitConfig[] unitConfig);

        Platoon Create(
            PlatoonSpawnContainer container,
            Transform platoonPosition,
            PlatoonType platoonType,
            UnitConfig[] unitConfig,
            List<Unit> unitsToRecreate);
    }
}