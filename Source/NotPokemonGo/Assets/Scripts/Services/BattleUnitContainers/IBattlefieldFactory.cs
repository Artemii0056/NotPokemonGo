using Characters;

namespace Services.BattleUnitContainers
{
    public interface IBattlefieldFactory
    {
        Battlefield Create(SpawnPositionConfig spawnPositionConfigFirstCommand, SpawnPositionConfig spawnPositionConfigSecondCommand);
    }
}