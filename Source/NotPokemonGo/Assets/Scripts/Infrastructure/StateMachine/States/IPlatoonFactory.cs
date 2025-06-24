using Characters;
using Platoons;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public interface IPlatoonFactory
    {
        Platoon Create(SpawnPositionConfig spawnPositionConfig, Transform platoonPosition,
            PlatoonType enemies, UnitConfig unitConfig);
    }
}