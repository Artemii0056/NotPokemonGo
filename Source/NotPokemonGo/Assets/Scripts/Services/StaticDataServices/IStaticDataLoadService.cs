using Characters.Configs;

namespace Services.StaticDataServices
{
    public interface IStaticDataLoadService
    {
        CharactersCatalogStaticData LoadCharacterCatalogStaticDatas();
    }
}