using Abilities.Configs;
using Armaments;
using Effects;
using Units;

namespace ReactionSystems
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
        
        public ReactionContext(Unit source, Unit target, Armament armament)
        {
            Source = source;
            Target = target;
            Armament = armament;
        }
        
        public Unit Source { get; }
        public Unit Target { get; }
        public EffectSetup Effect { get; }
        public AbilityPhase Phase { get; }
        public Armament Armament { get; }
    }
}