using Characters;
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
    }
}