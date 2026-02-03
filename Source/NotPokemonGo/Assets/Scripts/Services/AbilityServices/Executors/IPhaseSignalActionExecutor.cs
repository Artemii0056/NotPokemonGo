using System;
using Abilities.Configs;
using Units;

namespace Services.AbilityServices.Executors
{
    public interface IPhaseSignalActionExecutor
    {
        bool CanExecute(PhaseSignalAction action);

        void Execute(
            AbilityPhase phase,
            PhaseSignalAction action,
            Unit source,
            Unit target,
            PhaseGate finishGate,
            Action tryCompleteFinish = null);
    }
}
