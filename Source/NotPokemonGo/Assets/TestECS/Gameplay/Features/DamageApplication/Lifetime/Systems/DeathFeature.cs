using Infrastructure.Systems;

namespace TestECS.Gameplay.Features.DamageApplication.Lifetime.Systems
{
    public class DeathFeature : Feature
    {
        public DeathFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<MarkDeadSystem>());
        }
    }
}