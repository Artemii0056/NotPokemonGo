using Infrastructure.Systems;
using TestECS.Gameplay.Features.DamageApplication.Systems;

namespace TestECS.Gameplay.Features.DamageApplication
{
    public class DamageApplicationFeature : Feature
    {
        public DamageApplicationFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<ApplyDamageOnTargetsSystem>());
        }
    }
}