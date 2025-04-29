using Infrastructure.Systems;

namespace TestECS.Gameplay.Features.Statuses.Systems.StatusVisualsFeature.Visuals
{
    public class StatusVisualsFeature : Feature
    {
        public StatusVisualsFeature(ISystemFactory factory)
        {
            Add(factory.Create<ApplyPoisonVisualsSystem>());
            Add(factory.Create<UnapplyPoisonVisualsSystem>());
        }
    }
}