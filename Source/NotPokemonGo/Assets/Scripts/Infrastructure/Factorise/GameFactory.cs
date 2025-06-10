using AssetManagement;

namespace Infrastructure.Factorise
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