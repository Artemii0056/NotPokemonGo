using System;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.Signals;
using Abilities.Runtime;
using Cameras;
using Castaments;
using DefaultNamespace;
using Services.AbilityServices.Executors;
using Services.Audio;
using TimeServices;
using Units;
using Units.Movement;

namespace Services.AbilityServices
{
    /// <summary>
    /// Listens for phase signals and triggers configured actions.
    /// </summary>
    public sealed class AbilityPhaseService
    {
        private readonly PhaseFinishGate _finishGate = new();
        private readonly List<IPhaseSignalActionExecutor> _executors;

        private AbilityPhase _currentPhase;

        public event Action<ArmamentRequest> ArmamentRequested;

        public AbilityPhaseService(
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IParticleSpawner particleSpawner,
            IUnitMover unitMover,
            ICameraService camera, 
            IAudioService audio, 
            ITimeService time)
        {
            // The goal here is to keep AbilityPhaseService slim and open for extension.
            // To add new mechanics: add a new executor, not a new "HasX" branch here.
            _executors = new List<IPhaseSignalActionExecutor>
            {
                new ParticleActionExecutor(particleSpawner),
                new SoundActionExecutor(audio),   
                new TimeEffectExecutor(time),   
                new CameraActionExecutor(camera),
                new MoveActionExecutor(unitMover),
                new ArmamentActionExecutor(targetSelector, req => ArmamentRequested?.Invoke(req)),
                new CastamentActionExecutor(targetSelector, castamentApplicator)
            };
        }

        public void OnSignal(AbilityPhase phase, Unit source, Unit target, PhaseSignal signal)
        {
            if (phase == null || source == null || target == null)
                return;

            // Reset finish aggregation when phase changes.
            if (!ReferenceEquals(_currentPhase, phase))
            {
                _currentPhase = phase;
                _finishGate.Reset();
            }

            var actions = phase.SignalActions;
            if (actions == null || actions.Count == 0)
                return;

            bool anyAsync = false;

            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                if (action == null) continue;
                if (action.Signal != signal) continue;

                for (int e = 0; e < _executors.Count; e++)
                {
                    var executor = _executors[e];
                    if (!executor.CanExecute(action))
                        continue;

                    anyAsync |= executor.Execute(
                        phase,
                        action,
                        source,
                        target,
                        _finishGate,
                        () => TryCompleteFinish(source));
                }
            }

            if (anyAsync)
            {
                _finishGate.RequestFinish();
                TryCompleteFinish(source);
            }
        }

        private void TryCompleteFinish(Unit source)
        {
            if (!_finishGate.CanFinish)
                return;

            // Reset first, then emit, to avoid re-entrancy issues.
            _finishGate.Reset();

            source?.AnimatorController?.FlagSignal((int)PhaseSignal.Finish);
        }
    }
}
