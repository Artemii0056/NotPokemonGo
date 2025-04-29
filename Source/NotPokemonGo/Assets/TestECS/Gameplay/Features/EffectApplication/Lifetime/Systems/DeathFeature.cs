using Infrastructure.Systems;

namespace TestECS.Gameplay.Features.EffectApplication.Lifetime.Systems
{
    public class DeathFeature : Feature
    {
        public DeathFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<MarkDeadSystem>());
            Add(systemFactory.Create<UnapplyStatusesOfDeadTargetSystem>());
        }
    }
}