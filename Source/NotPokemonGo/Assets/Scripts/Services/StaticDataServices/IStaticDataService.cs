using Characters;
using Characters.Configs;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
        SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType);
    }
}