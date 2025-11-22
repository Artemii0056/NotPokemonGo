using Effects;
using Units;

namespace Infrastructure.ReactionSystem
{
    public class ReactionContext
    {
        public ReactionContext(Unit source, Unit target, EffectSetup effect, AbilityPhase phase)
        {
            Source = source;
            Target = target;
            Effect = effect;
            Phase = phase;
        }
        
        public Unit Source { get; }
        public Unit Target { get; }
        public EffectSetup Effect { get; }
        public AbilityPhase Phase { get; }
    }
}