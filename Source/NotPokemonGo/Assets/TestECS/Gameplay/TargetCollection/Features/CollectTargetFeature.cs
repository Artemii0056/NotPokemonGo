using Infrastructure.Systems;
using TestECS.Gameplay.TargetCollection.Systems;

namespace TestECS.Gameplay.TargetCollection.Features
{
    public class CollectTargetFeature : Feature
    {
        public CollectTargetFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<CollectTargetsIntervalSystem>());
            
            Add(systemFactory.Create<CastForTargetNoLimitSystem>());
            Add(systemFactory.Create<CastForTargetWithLimitSystem>());
            
            Add(systemFactory.Create<CleanupTargetBufferSystem>());
        }
    }
}