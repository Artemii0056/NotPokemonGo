using Characters;

namespace Battlefields
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(SpawnPositionConfig spawnPositionConfigFirstCommand, SpawnPositionConfig spawnPositionConfigSecondCommand);
    }
}