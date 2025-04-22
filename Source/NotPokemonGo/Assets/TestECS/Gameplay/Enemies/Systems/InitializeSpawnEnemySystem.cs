using Code.Entity;
using Entitas;

namespace TestECS.Gameplay.Enemies.Systems
{
    public class InitializeSpawnEnemySystem : IInitializeSystem
    {
        public void Initialize()
        {
            CreateEntity.Empty()
                .AddSpawnTimer(GameplayConstants.EnemySpawnTimer);
        }
    }
}