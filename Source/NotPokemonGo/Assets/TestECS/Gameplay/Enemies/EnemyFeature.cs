using Infrastructure.Systems;
using TestECS.Gameplay.Code.Common.Systems;
using TestECS.Gameplay.Enemies.Systems;

namespace TestECS.Gameplay.Enemies
{
    public class EnemyFeature : Feature
    {
        public EnemyFeature(ISystemFactory factory)
        {
            Add(factory.Create<InitializeSpawnEnemySystem>());
            
            Add(factory.Create<EnemySpawnSystem>());
            
            Add(factory.Create<ChaseHeroSystem>());
            Add(factory.Create<EnemyDeathSystem>());
            
            Add(factory.Create<FinalizeEnemyDeathProcessingSystem>());
        }
    }
}