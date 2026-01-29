using System;
using System.Collections.Generic;
using Abilities.Configs;
using Abilities.Signals;
using Castaments;
using DefaultNamespace;
using Services.AbilityServices.Executors;
using Services.Audio;
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
            _executors = new List<IPhaseSignalActionExecutor>
            {
                new ParticleActionExecutor(particleSpawner), //Паттерн Команда - Undo
                new SoundActionExecutor(audio),
                new TimeEffectExecutor(time),
                new CameraActionExecutor(camera),
                new MoveActionExecutor(unitMover),
                new ArmamentActionExecutor(targetSelector, req => ArmamentRequested?.Invoke(req)),
                new CastamentActionExecutor(targetSelector, castamentApplicator)
            };
        }

        public void BindFinishCheck(Action requestFinishCheck)
        {
            _requestFinishCheck = requestFinishCheck;
            Debug.Log("Binding finish check");
        }

        public void RequestFinishCheck() =>
            TryCompleteFinish();

        public void OnSignal(AbilityPhase phase, Unit source, Unit target, PhaseSignal signal)
        {
            Debug.Log("OnSignal");
            
            if (phase == null || source == null || target == null)
                return;

            if (ReferenceEquals(_currentPhase, phase) == false)
            {
                _currentPhase = phase;
                _finishGate.Reset();
            }

            var actions = phase.SignalActions;

            if (actions == null || actions.Count == 0)
                return;

            for (int i = 0; i < actions.Count; i++)
            {
                PhaseSignalAction action = actions[i];

                if (action == null || action.Signal != signal)
                    continue;

                for (int index = 0; index < _executors.Count; index++)
                {
                    IPhaseSignalActionExecutor executor = _executors[index];

                    if (executor.CanExecute(action) == false)
                        continue;

                    Debug.Log("OnSignal");
                    Debug.Log("Executing action: " + executor.GetType().Name);
                    executor.Execute(phase, action, source, target, _finishGate);
                }
            }

            TryCompleteFinish();
        }

        private void TryCompleteFinish()
        {
            if (_currentPhase == null)
                return;

            // if (!_finishGate.IsOpen)
            //     return;
            
            Debug.Log("!!!!!");

            _requestFinishCheck?.Invoke();
        }
    }
}
