using Characters;
using DefaultNamespace;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public interface IPlatoonFactory
    {
        Platoon Create(SpawnPositionConfig spawnPositionConfigFirstCommand, Transform platoonPosition);
    }
}