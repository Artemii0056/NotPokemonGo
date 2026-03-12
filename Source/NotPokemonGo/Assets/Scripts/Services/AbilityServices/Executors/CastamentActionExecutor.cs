using System;
using System.Collections.Generic;
using System.Linq;
using Abilities.Configs;
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

        public bool CanExecute(PhaseSignalAction action) => 
            action != null && action.HasCastament;

        public void Execute(AbilityPhase phase, PhaseSignalAction action, Unit source, Unit target, PhaseGate finishGate)
        {
            if (_targetSelector == null || _castamentApplicator == null)
                return;

            IReadOnlyList<Unit> targets = new []{target };
            
            if (targets == null || targets.Count == 0)
                return;

            _castamentApplicator.Apply(action.CastamentSetup, source, targets.ToArray());
        }
    }
}
