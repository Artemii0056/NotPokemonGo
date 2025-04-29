using Infrastructure.Systems;
using TestECS.Gameplay.Features.EffectApplication.Systems;
using TestECS.Gameplay.Features.Statuses;

namespace TestECS.Gameplay.Features.EffectApplication
{
    public class EffectApplicationFeature : Feature
    {
        public EffectApplicationFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<ApplyEffectsOnTargetsSystem>());
            Add(systemFactory.Create<ApplyStatusesOnTargetsSystem>());
        }
    }
}