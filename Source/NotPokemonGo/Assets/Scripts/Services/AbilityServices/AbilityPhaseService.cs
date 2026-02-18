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
using UnityEngine;

namespace Services.AbilityServices
{
    public sealed class AbilityPhaseService
    {
        private readonly PhaseGate _finishGate = new();
        private readonly List<IPhaseSignalActionExecutor> _executors;

        private AbilityPhase _currentPhase;

        private Action _requestFinishCheck;

        private int _phaseVersion;

        public event Action<ArmamentRequest> ArmamentRequested;

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
        }

        public void BindFinishCheck(Action requestFinishCheck)
        {
            _requestFinishCheck = requestFinishCheck;
        }

        public void BeginPhase(AbilityPhase phase)
        {
            _currentPhase = phase;
            _finishGate.Reset();

            _phaseVersion++;
        }

        public IDisposable AcquireFinishToken(string tag = null)
        {
            var t = _finishGate.Acquire(tag);
            Debug.Log($"[Gate] AcquireFinishToken tag={tag} -> {(t == null ? "NULL" : t.GetType().Name)} pending={_finishGate.Pending}");
            return t;
        }

        public void RequestFinishCheck() => 
            TryCompleteFinish();

        public void OnSignal(AbilityPhase phase, Unit source, Unit target, PhaseSignal signal)
        {
            if (phase == null || source == null || target == null)
                return;

            if (!ReferenceEquals(_currentPhase, phase))
                return;

            var actions = phase.SignalActions;
            
            if (actions == null || actions.Count == 0)
            {
                TryCompleteFinish();
                return;
            }

            for (int i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                
                if (action == null || action.Signal != signal)
                    continue;

                for (int j = 0; j < _executors.Count; j++)
                {
                    var executor = _executors[j];
                    
                    if (!executor.CanExecute(action))
                        continue;

                    int capturedVersion = _phaseVersion;

                    executor.Execute(
                        phase, action, source, target,
                        _finishGate,
                        () =>
                        {
                            if (capturedVersion != _phaseVersion)
                                return;

                            TryCompleteFinish();
                        });
                }
            }

            TryCompleteFinish();
        }

        private void TryCompleteFinish()
        {
            if (_currentPhase == null)
                return;

            if (!_finishGate.IsOpen)
                return;

            Action cb = _requestFinishCheck;
            
            if (cb == null)
                return;

            cb.Invoke();
        }
    }
}