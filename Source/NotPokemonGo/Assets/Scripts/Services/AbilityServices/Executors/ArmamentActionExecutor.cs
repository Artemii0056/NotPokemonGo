using System;
using Abilities.Configs;
using Abilities.Runtime;
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

        public bool Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseFinishGate finishGate, Action tryCompleteFinish)
        {
            if (_targetSelector == null || _raise == null)
                return false;

            var targets = _targetSelector.GetTargets(action.TargetMode, target);
            
            if (targets == null || targets.Count == 0)
                return false;

            _raise(new ArmamentRequest(phase, action, source, targets.ToArray()));
            return false;
        }
    }
}
