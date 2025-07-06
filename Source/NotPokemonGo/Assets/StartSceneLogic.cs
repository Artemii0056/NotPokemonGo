using System.ComponentModel.Design;
using Services.AssetManagement;
using Services.StaticDataServices;

namespace DefaultNamespace
{
    public class StartSceneLogic
    {
        private IStaticDataService _staticDataService;
        private IResourceLoader _resourceLoader;

        public StartSceneLogic(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
    }
}