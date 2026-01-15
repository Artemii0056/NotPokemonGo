using System;
using Abilities.Configs;
using Abilities.Signals;
using Services.AbilityServices;

namespace Units.AnimationControllers
{
    public sealed class UnitAnimatorTrigger : IDisposable
    {
        public AbilityPhaseService PhaseService { get; }

        private readonly Unit _unit;
        private readonly AnimatorController _anim;

        private AbilityPhase _phase;
        private Unit _target;

        public UnitAnimatorTrigger(Unit unit, AnimatorController anim, AbilityPhaseService phaseService)
        {
            _unit = unit;
            _anim = anim;
            PhaseService = phaseService;

            _anim.Signal += OnSignal;
        }

        public void Dispose()
        {
            _anim.Signal -= OnSignal;
        }

        public void SetTarget(Unit target) => _target = target;
        public void SetPhase(AbilityPhase phase) => _phase = phase;

        private void OnSignal(int id)
        {
            var signal = PhaseSignalUtil.FromInt(id);
            if (signal == PhaseSignal.None) return;

            if (_phase == null || _unit == null || _target == null)
                return;

            PhaseService.OnSignal(_phase, _unit, _target, signal);
        }
    }
}