using Abilities.Configs;
using Abilities.Signals;

namespace Abilities.Runtime.Policies
{
    public sealed class FinishSignalPolicy : AbilityPolicyBase //TODO Delete
    {
        private bool _finishReceived;

        public override void OnPhaseStart(AbilityContext ctx, AbilityPhase phase)
            => _finishReceived = false;

        public override void OnSignal(AbilityContext ctx, PhaseSignal signal)
        {
            if (signal == PhaseSignal.Finish)
                _finishReceived = true;
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
            => _finishReceived;
    }
}
