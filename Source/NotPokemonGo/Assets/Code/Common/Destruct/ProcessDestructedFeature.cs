using Code.Common.Destruct.Systems;
using Infrastructure.Systems;

namespace Code.Common.Destruct
{
    public class ProcessDestructedFeature : Feature
    {
        public ProcessDestructedFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<SelfDestructTimerSystem>());
            
            Add(systemFactory.Create<DestructedViewSystem>());
            Add(systemFactory.Create<CleanupGameDestructedSystem>());
        }
    }
}