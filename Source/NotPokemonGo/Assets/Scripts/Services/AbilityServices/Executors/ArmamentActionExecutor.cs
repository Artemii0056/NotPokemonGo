using System;
using Abilities.Configs;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class ArmamentActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly ITargetSelector _targetSelector;
        private readonly Action<ArmamentRequest> _raise;

        public ArmamentActionExecutor(ITargetSelector targetSelector, Action<ArmamentRequest> raise)
        {
            _targetSelector = targetSelector;
            _raise = raise;
        }

        public bool CanExecute(PhaseSignalAction action) => 
            action != null && action.HasArmament;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate)
        {
            if (_targetSelector == null || _raise == null)
                return;

            var targets = _targetSelector.GetTargets(action.TargetMode, target);

            if (targets == null || targets.Count == 0)
                return;

            _raise(new ArmamentRequest(phase, action, source, targets.ToArray()));
        }
    }
}