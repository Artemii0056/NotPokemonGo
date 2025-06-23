using Characters;

namespace Infrastructure.StateMachines.States
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(SpawnPositionConfig spawnPositionConfigFirstCommand, SpawnPositionConfig spawnPositionConfigSecondCommand);
    }
}