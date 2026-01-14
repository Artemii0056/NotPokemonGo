using Abilities.Configs;
using Armaments;
using Units;

namespace Services.AbilityServices
{
    public readonly struct ArmamentRequest
    {
        public ArmamentRequest(ArmamentSetup setup, Unit source, Unit[] targets, AbilityPhase phase)
        {
            Setup = setup;
            Source = source;
            Targets = targets;
            Phase = phase;
        }

        public ArmamentSetup Setup { get; }
        public Unit Source { get; }
        public Unit[] Targets { get; }
        public AbilityPhase Phase { get; }
    }
}