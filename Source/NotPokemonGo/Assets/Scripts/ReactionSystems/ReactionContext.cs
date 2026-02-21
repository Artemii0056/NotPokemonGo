using System;
using Abilities.Configs;
using Armaments;
using Armaments.Movers;
using Effects;
using Units;

namespace ReactionSystems
{
    public class ReactionContext
    {
        public Unit Source;
        public Unit Target;
        public EffectSetup Effect;
        public AbilityPhase Phase;
        public Armament Armament;
        public bool WasReflected;
        public Action<IArmamentMover> SpawnShot;
    }
}