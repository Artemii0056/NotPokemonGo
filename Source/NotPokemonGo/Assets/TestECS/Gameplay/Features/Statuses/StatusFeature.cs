using Infrastructure.Systems;
using TestECS.Gameplay.Features.Statuses.Systems;
using TestECS.Gameplay.Features.Statuses.Systems.StatusVisualsFeature.Visuals;

namespace TestECS.Gameplay.Features.Statuses
{
    public class StatusFeature : Feature
    {
        public StatusFeature(ISystemFactory factory)
        {
            Add(factory.Create<StatusDurationSystem>());
            Add(factory.Create<PeriodicDamageStatusSystem>());
            
            Add(factory.Create<StatusVisualsFeature>());
            
            Add(factory.Create<CleanupUnappliedStatuses>());
        }
    }
}