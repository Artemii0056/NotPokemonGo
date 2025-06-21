using Characters;
using DefaultNamespace;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public interface IPlatoonFactory
    {
        Platoon Create(SpawnPositionConfig spawnPositionConfig, Transform platoonPosition,
            PlatoonType enemies, CharacterConfig characterConfig);
    }
}