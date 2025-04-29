using Entitas;

namespace TestECS.Gameplay.Features.Effects.Systems
{
    public class ProcessDamageEffectSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _effects;

        public ProcessDamageEffectSystem(GameContext gameContext)
        {
            _effects = gameContext.GetGroup(GameMatcher
                .AllOf(
                    GameMatcher.DamageEffect,
                    GameMatcher.EffectValue,
                    GameMatcher.TargetId));
        }

        public void Execute()
        {
            foreach (GameEntity effect in _effects)
            {
                GameEntity target = effect.Target();
                
                effect.isProcessed = true;

                if (target.isDead)
                    continue;

                target.ReplaceCurrentHealthPoint(target.CurrentHealthPoint - effect.EffectValue);

                if (target.hasDamageTakenAnimator)
                    target.DamageTakenAnimator.PlayDamageTaken();
            }
        }
    }
}