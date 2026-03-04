using System;
using System.Collections.Generic;
using System.Threading;
using Abilities.Configs;
using Abilities.Signals;
using Castaments;
using Cysharp.Threading.Tasks;
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

        private UniTaskCompletionSource _phaseCompletedTcs;
        private bool _finishSignalReceived;

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

        public void BeginPhase(AbilityPhase phase, int ownerId = 0)
        {
            _currentPhase = phase;
            _finishGate.Reset(ownerId);

            _finishSignalReceived = false;
            _phaseCompletedTcs = new UniTaskCompletionSource();
        }
        
        public IDisposable Acquire(string tag = null) =>
            _finishGate.Acquire(tag);

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

            if (signal == PhaseSignal.Finish)
            {
                _finishSignalReceived = true;
                TryCompletePhase();
            }

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

            TryCompletePhase();
        }
        
        public void ForceFinish(string reason = null)
        {
            if (_finishSignalReceived)
                return;

            _finishSignalReceived = true;

            if (!string.IsNullOrEmpty(reason))
                DumpGateDebug($"[PhaseService] ForceFinish reason={reason}");

            TryCompletePhase();
        }

        public UniTask WaitPhaseCompletionAsync(CancellationToken token)
        {
            if (_phaseCompletedTcs == null)
                return UniTask.CompletedTask;

            return _phaseCompletedTcs.Task.AttachExternalCancellation(token);
        }

        public void DumpGateDebug(string prefix = null, int maxEntries = 32) =>
            _finishGate.DumpToLog(prefix, maxEntries);

        private void OnGateOpened()
        {
            PhaseCompleted?.Invoke();
            TryCompletePhase();
        }

        private void TryCompletePhase()
        {
            if (!_finishSignalReceived)
                return;

            if (!_finishGate.IsOpened)
                return;

            _phaseCompletedTcs?.TrySetResult();
        }
    }
}
