using Effects;
using Units;

namespace Infrastructure.ReactionSystem
{
    public class ReactionContext
    {
        public ReactionContext(Unit source, Unit target, EffectSetup effect)
        {
            Source = source;
            Target = target;
            Effect = effect;
        }
        
        public Unit Source { get; }
        public Unit Target { get; }
        public EffectSetup Effect { get; }
    }
}