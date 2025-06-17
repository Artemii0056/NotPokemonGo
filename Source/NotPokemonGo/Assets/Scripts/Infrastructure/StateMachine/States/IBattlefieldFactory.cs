using Characters;
using DefaultNamespace;

namespace Infrastructure.StateMachine.States
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(SpawnPositionConfig spawnPositionConfigFirstCommand, SpawnPositionConfig spawnPositionConfigSecondCommand);
    }
}