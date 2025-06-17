using Characters;

public class LocationTypeInfo
{
    public readonly SpawnPositionType SpawnPositionTypeFirstCommand;
    public readonly SpawnPositionType SpawnPositionTypeSecondCommand;

    public LocationTypeInfo(SpawnPositionType spawnPositionTypeFirstCommand, SpawnPositionType spawnPositionTypeSecondCommand)
    {
        SpawnPositionTypeFirstCommand = spawnPositionTypeFirstCommand;
        SpawnPositionTypeSecondCommand = spawnPositionTypeSecondCommand;
    }
}