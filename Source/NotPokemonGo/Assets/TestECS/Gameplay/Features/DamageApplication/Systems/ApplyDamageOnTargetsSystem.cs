using Entitas;

namespace TestECS.Gameplay.Features.DamageApplication.Systems
{
    public class ApplyDamageOnTargetsSystem : IExecuteSystem
    {
        private readonly GameContext _gameContext;
        private readonly IGroup<GameEntity> _dealers;

        public ApplyDamageOnTargetsSystem(GameContext gameContext)
        {
            _gameContext = gameContext;
            _dealers = gameContext.GetGroup(GameMatcher.AllOf(
                GameMatcher.TargetsBuffer,
                GameMatcher.Damage));
        }

        public void Execute()
        {
            foreach (GameEntity dealer in _dealers)
            foreach (int targetId in dealer.TargetsBuffer)
            {
                GameEntity target = _gameContext.GetEntityWithId(targetId);

                if (target.hasCurrentHealthPoint)
                {
                    target.ReplaceCurrentHealthPoint(target.CurrentHealthPoint - dealer.Damage);

                    if (target.hasDamageTakenAnimator) 
                        target.DamageTakenAnimator.PlayDamageTaken();
                }
            }
        }
    }
}