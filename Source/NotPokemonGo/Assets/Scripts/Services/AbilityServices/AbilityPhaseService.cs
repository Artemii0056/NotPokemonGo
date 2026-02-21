using System;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.Signals;
using Castaments;
using Services.AbilityServices.Executors;
using Services.AudioServices;
using Services.Cameras;
using Spawners;
using TimeServices;
using Units;
using Units.Movement;

namespace Services.AbilityServices
{
    public sealed class AbilityPhaseService
    {
        private readonly PhaseGate _finishGate = new();
        private readonly List<IPhaseSignalActionExecutor> _executors;

        private AbilityPhase _currentPhase;

        public event Action PhaseCompleted;

        public AbilityPhaseService(
            ICastamentApplicator castamentApplicator,
            ITargetSelector targetSelector,
            IParticleSpawner particleSpawner,
            IUnitMover unitMover,
            ICameraService camera,
            IAudioService audio,
            ITimeService time,
            ICameraShakeService cameraShake)
        {
            _executors = new List<IPhaseSignalActionExecutor>
            {
                new ParticleActionExecutor(particleSpawner),
                new SoundActionExecutor(audio),
                new TimeEffectExecutor(time),
                new CameraActionExecutor(camera),
                new MoveActionExecutor(unitMover),
                new ArmamentActionExecutor(targetSelector, req => ArmamentRequested?.Invoke(req)),
                new CastamentActionExecutor(targetSelector, castamentApplicator),
                new CameraShakeActionExecutor(cameraShake)
            };

            _finishGate.Opened += OnGateOpened;
        }
        
        public event Action<ArmamentRequest> ArmamentRequested;

        public void BeginPhase(AbilityPhase phase)
        {
            _currentPhase = phase;
            _finishGate.Reset();
        }

        public void OnSignal(
            AbilityPhase phase,
            Unit source,
            Unit target,
            PhaseSignal signal)
        {
            if (phase == null || source == null || target == null)
                return;

            if (!ReferenceEquals(_currentPhase, phase))
                return;

            List<PhaseSignalAction> actions = phase.SignalActions;

            if (actions == null || actions.Count == 0)
                return;

            using var rootToken = _finishGate.Acquire("SignalRoot");

            for (int i = 0; i < actions.Count; i++)
            {
                PhaseSignalAction action = actions[i];

                if (action == null || action.Signal != signal)
                    continue;

                for (int j = 0; j < _executors.Count; j++)
                {
                    var executor = _executors[j];

                    if (!executor.CanExecute(action))
                        continue;

                    executor.Execute(
                        phase,
                        action,
                        source,
                        target,
                        _finishGate);
                }
            }
        }

        private void OnGateOpened() => 
            PhaseCompleted?.Invoke();
    }
}
