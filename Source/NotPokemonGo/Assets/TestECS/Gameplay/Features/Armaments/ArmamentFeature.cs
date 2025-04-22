using Infrastructure.Systems;

namespace TestECS.Gameplay.Features.Armaments
{
    public class ArmamentFeature : Feature
    {
        public ArmamentFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<MarkProcessedOnTargetLimitExceededSystem>());
            Add(systemFactory.Create<FinalizeProcessedArmamentsSystem>());
        }
    }
}