using Infrastructure.Systems;
using TestECS.Gameplay.Features.CharacterStats.Systems;

namespace TestECS.Gameplay.Features.CharacterStats
{
    public class StatsFeature : Feature
    {
        public StatsFeature(ISystemFactory factory)
        {
            Add(factory.Create<StatChangeSystem>());
            
            Add(factory.Create<ApplySpeedFromStatsSystem>());
        }
    }
}