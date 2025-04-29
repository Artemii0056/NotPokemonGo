using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Features.Armaments
{
    public class MoveToTargetAndMarkReachedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _bouncingBolts;
        private readonly IGroup<GameEntity> _enemies;

        public MoveToTargetAndMarkReachedSystem(GameContext gameContext)
        {
            _bouncingBolts = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.BouncingArmament,
                    GameMatcher.TargetsBuffer,
                    GameMatcher.TargetLimit));

            _enemies = gameContext
                .GetGroup(GameMatcher
                    .AllOf(
                        GameMatcher.Enemy,
                        GameMatcher.WorldPosition,
                        GameMatcher.Id));
        }

        public void Execute()
        {
            foreach (GameEntity armament in _bouncingBolts)
            foreach (GameEntity enemy in _enemies)
            {
                if (armament.hasTargetId == false)
                    continue;

                if (armament.TargetId == enemy.Id)
                {
                    armament.ReplaceDirection((enemy.WorldPosition - armament.WorldPosition).normalized);

                    if (Vector3.Distance(enemy.WorldPosition, armament.WorldPosition) < 0.5f)
                    {
                        armament.isReachedToTarget = true;
                        armament.isNeedTarget = true;
                    }
                }
            }
        }
    }
}