using Characters;
using Platoons;
using UnityEngine;

namespace Infrastructure.StateMachines.States
{
    public interface IPlatoonFactory
    {
        Platoon Create(SpawnPositionConfig spawnPositionConfig, Transform platoonPosition,
            PlatoonType enemies, CharacterConfig characterConfig);
    }
}