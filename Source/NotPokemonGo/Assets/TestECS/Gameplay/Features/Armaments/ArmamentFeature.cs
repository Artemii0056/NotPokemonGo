using Infrastructure.Systems;
using TestECS.Gameplay.Features.Abilities.System;

namespace TestECS.Gameplay.Features.Armaments
{
    public class ArmamentFeature : Feature
    {
        public ArmamentFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<SetTargetSystem>());
            Add(systemFactory.Create<MoveToTargetAndMarkReachedSystem>());
            Add(systemFactory.Create<MarkDeathOrSwitchTargetSystem>());
            
            Add(systemFactory.Create<MarkProcessedOnTargetLimitExceededSystem>());
            Add(systemFactory.Create<FinalizeProcessedArmamentsSystem>());
        }
    }
}