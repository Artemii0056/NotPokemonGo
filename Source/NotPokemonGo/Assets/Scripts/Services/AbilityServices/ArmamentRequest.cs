using Abilities.Configs;
using Armaments;
using Units;

namespace Services.AbilityServices
{
    public readonly struct ArmamentRequest
    {
        public readonly AbilityPhase Phase;
        public readonly PhaseSignalAction Action;
        public readonly Unit Source;
        public readonly Unit[] Targets;

        public ArmamentRequest(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit[] targets)
        {
            Phase = phase;
            Action = action;
            Source = source;
            Targets = targets;
        }

        public ArmamentSetup Setup => Action.ArmamentSetup;
    }
}