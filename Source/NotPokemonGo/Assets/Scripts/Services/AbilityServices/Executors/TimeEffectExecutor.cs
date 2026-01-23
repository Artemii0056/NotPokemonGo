using Abilities.Configs;
using Abilities.Runtime;
using TimeServices;
using Units;

namespace Services.AbilityServices.Executors
{
    public sealed class TimeEffectExecutor : IPhaseSignalActionExecutor
    {
        private readonly ITimeService _time;

        public TimeEffectExecutor(ITimeService time) => 
            _time = time;

        public bool CanExecute(PhaseSignalAction action)
            => action != null && action.HasTimeEffect;

        public bool Execute(
            AbilityPhase phase,
            PhaseSignalAction action,
            Unit source,
            Unit target,
            PhaseFinishGate gate,
            System.Action onComplete)
        {
            if (_time == null)
            {
                onComplete?.Invoke();
                return false;
            }

            _time.HitStop(action.TimeScale, action.TimeDuration);

            onComplete?.Invoke();
            return false;
        }
    }
}