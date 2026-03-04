using System;
using Abilities.Configs;
using Abilities.Signals;
using Services.AbilityServices;

namespace Units.AnimationControllers
{
    public sealed class UnitAnimatorTrigger : IDisposable 
    {
        public AbilityPhaseService PhaseService { get; }

        public event Action<PhaseSignal> SignalRaised;

        public AbilityPhase CurrentPhase => _phase;
        public Unit CurrentTarget => _target;
        public Unit Owner => _unit;

        private readonly Unit _unit;
        private readonly AnimatorController _animatorController;

        private AbilityPhase _phase;
        private Unit _target;

        public UnitAnimatorTrigger(Unit unit, AnimatorController animatorController, AbilityPhaseService phaseService)
        {
            _unit = unit;
            _animatorController = animatorController;
            PhaseService = phaseService;

            _animatorController.Signal += OnSignal;
        }

        public void Dispose() => 
            _animatorController.Signal -= OnSignal;

        public void SetTarget(Unit target) => 
            _target = target;
        
        public void SetPhase(AbilityPhase phase) =>
            _phase = phase;

        private void OnSignal(int id)
        {
            var signal = PhaseSignalUtil.FromInt(id);
            
            if (signal == PhaseSignal.None)
                return;

            SignalRaised?.Invoke(signal);

            if (_phase == null || _unit == null || _target == null)
                return;

            PhaseService.OnSignal(_phase, _unit, _target, signal);
        }
    }
}