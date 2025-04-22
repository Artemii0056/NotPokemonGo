using Infrastructure.Systems;
using Infrastructure.View.Systems;

namespace Infrastructure.View.Features
{
    public class BindViewFeature : Feature
    {
        public BindViewFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<BindEntityViewFromPathSystem>());
            Add(systemFactory.Create<BindEntityViewFromPrefabSystem>());
        }
    }
}