using Entitas;
using TestECS.Gameplay.Enemies.Factory;
using TestECS.TimeService;
using UnityEngine;

namespace TestECS.Gameplay.Enemies.Systems
{
    public class EnemySpawnSystem : IExecuteSystem
    {
        private readonly ITimeService _time;
        private readonly IEnemyFactory _factory;
        private readonly IGroup<GameEntity> _timers;
        private readonly IGroup<GameEntity> _heroes;

        public EnemySpawnSystem(GameContext gameContext, ITimeService time, IEnemyFactory factory)
        {
            _time = time;
            _factory = factory;

            _timers = gameContext.GetGroup(GameMatcher.SpawnTimer);

            _heroes = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Hero,
                    GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (GameEntity hero in _heroes)
            foreach (GameEntity timer in _timers)
            {
                timer.ReplaceSpawnTimer(timer.SpawnTimer - _time.DeltaTime);

                if (timer.SpawnTimer <= 0)
                {
                    timer.ReplaceSpawnTimer(GameplayConstants.EnemySpawnTimer);
                    _factory.Create(EnemyTypeId.First, at: RandomSpawnPosition(hero.WorldPosition));
                }
            }
        }

        private Vector3 RandomSpawnPosition(Vector3 heroWorldPosition)
        {
            return new Vector3(
                heroWorldPosition.x + Random.Range(-20, 20),
                0.5f,
                heroWorldPosition.z + Random.Range(-20, 20));
        }
    }
}