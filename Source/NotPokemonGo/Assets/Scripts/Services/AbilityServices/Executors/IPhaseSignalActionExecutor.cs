using System;
using Abilities.Configs;
using Abilities.Runtime;
using Units;

namespace Services.AbilityServices.Executors
{
    public interface IPhaseSignalActionExecutor
    {
        bool CanExecute(PhaseSignalAction action);

        bool Execute(
            AbilityPhase phase,
            PhaseSignalAction action,
            Unit source,
            Unit target,
            PhaseFinishGate finishGate,
            Action tryCompleteFinish = null);
    }
}
