using Characters;
using UI.SpawnPositions;
using UnityEngine;

namespace Platoons
{
    public interface IPlatoonFactory
    {
        Platoon Create(
            SpawnPositionConfig spawnPositionConfig,
            Transform platoonPosition,
            PlatoonType enemies,
            UnitConfig[] unitConfig);

        Platoon Create2(
            PlatoonSpawnContainer container,
            Transform platoonPosition,
            PlatoonType platoonType,
            UnitConfig[] unitConfig);
    }
}