using System;
using Abilities.Configs;
using Abilities.Runtime;
using Castaments;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class CastamentActionExecutor : IPhaseSignalActionExecutor
    {
        private readonly ITargetSelector _targetSelector;
        private readonly ICastamentApplicator _castamentApplicator;

        public CastamentActionExecutor(ITargetSelector targetSelector, ICastamentApplicator castamentApplicator)
        {
            _targetSelector = targetSelector;
            _castamentApplicator = castamentApplicator;
        }

        public bool CanExecute(PhaseSignalAction action) => action != null && action.HasCastament;

        public bool Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseFinishGate finishGate, Action tryCompleteFinish)
        {
            if (_targetSelector == null || _castamentApplicator == null)
                return false;

            var targets = _targetSelector.GetTargets(action.TargetMode, target);
            if (targets == null || targets.Count == 0)
                return false;

            _castamentApplicator.Apply(action.CastamentSetup, source, targets.ToArray());
            return false;
        }
    }
}
