using Infrastructure.Scripts.AssetManagement;

namespace Infrastructure.Scripts.Factorise
{
    public class GameFactory
    {
        private IResourceLoader _resourceLoader;

        public GameFactory(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }
    }
}