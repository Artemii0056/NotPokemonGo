namespace TestECS.Gameplay.Features.Effects
{
    public static class EffectEntityExtensions
    {
        private static GameContext Context => Contexts.sharedInstance.game;

        public static GameEntity Producer(this GameEntity effect)
        {
            if (effect.hasProducerId)
                return Context.GetEntityWithId(effect.ProducerId);
            else
                return null;
        }

        public static GameEntity Target(this GameEntity effect)
        {
            if (effect.hasProducerId)
                return Context.GetEntityWithId(effect.TargetId);
            else
                return null;
        }
    }
}