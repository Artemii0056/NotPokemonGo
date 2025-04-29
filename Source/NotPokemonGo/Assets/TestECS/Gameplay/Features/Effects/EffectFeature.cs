using Infrastructure.Systems;
using TestECS.Gameplay.Features.Effects.Systems;

namespace TestECS.Gameplay.Features.Effects
{
    public class EffectFeature : Feature
    {
        public EffectFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<RemoveEffectsWithoutTargetsSystem>());
            
            Add(systemFactory.Create<ProcessDamageEffectSystem>());
            
            Add(systemFactory.Create<CleanupProcessedEffects>());
        }
    }
}