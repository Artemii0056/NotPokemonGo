using Code.Extensions;
using Entitas;
using UnityEngine;

namespace TestECS.Gameplay.Features.Abilities.System
{
    public class SetTargetSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _bouncingBolts;
        private readonly IGroup<GameEntity> _enemies;

        public SetTargetSystem(GameContext gameContext)
        {
            _bouncingBolts = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Armament,
                    GameMatcher.BouncingArmament));

            _enemies = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.Enemy,
                    GameMatcher.WorldPosition));
        }

        public void Execute()
        {
            foreach (GameEntity bolt in _bouncingBolts)
            {
                if (bolt.isNeedTarget)
                {
                    if (_enemies.count > 1)
                        InitializeBolt(bolt);
                    else
                        bolt.isDestructed = true;
                }
            }
        }

        private void InitializeBolt(GameEntity bolt)
        {
            int id = FirstAvailableTargetId();

            bolt.ReplaceTargetId(id)
                .With(x => x.isNeedTarget = false)
                .With(x => x.isMoving = true);
        }

        private int FirstAvailableTargetId()
        {
            GameEntity[] list = _enemies.GetEntities();
            
            return list[Random.Range(0, list.Length)].Id;
        }
    }
}