using Characters;
using UI.SpawnPositions;
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
    }
}