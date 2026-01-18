using System;
using Abilities.Configs;
using Abilities.Runtime;
using Units;

namespace Services.AbilityServices.Executors
{
    /// <summary>
    /// Executes a part of <see cref="PhaseSignalAction"/>. Designed to keep AbilityPhaseService slim.
    /// </summary>
    public interface IPhaseSignalActionExecutor
    {
        bool CanExecute(PhaseSignalAction action);

        /// <summary>
        /// Executes the action. Return true if this executor started an async process that should delay phase finish.
        /// </summary>
        bool Execute(
            AbilityPhase phase,
            PhaseSignalAction action,
            Unit source,
            Unit target,
            PhaseFinishGate finishGate,
            Action tryCompleteFinish);
    }
}
