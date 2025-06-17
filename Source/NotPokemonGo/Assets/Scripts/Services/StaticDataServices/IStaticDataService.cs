using Characters;
using Characters.Configs;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
        SpawnPositionConfig GetLocationTypeConfig(SpawnPositionType spawnPositionType);
    }
}