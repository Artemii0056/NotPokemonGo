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

        // handler binds TryFinishPhase сюда
        private Action _requestFinishCheck;

        // защита от спама/поздних коллбеков
        private bool _finishRequested;
        private int _phaseVersion;
        
        private int _lastRequestFrame = -1;

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
                new ParticleActionExecutor(particleSpawner),
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
        }

        /// <summary>
        /// ВАЖНО: вызывать на старте каждой фазы ДО policy.OnPhaseStart.
        /// Иначе policy может взять токен, а потом первый OnSignal сделает Reset и "сотрёт" pending.
        /// </summary>
        public void BeginPhase(AbilityPhase phase)
        {
            _currentPhase = phase;
            _finishGate.Reset();

            _finishRequested = false;
            _phaseVersion++;
        }

        /// <summary>
        /// Политики (например QTE) могут удерживать фазу токеном.
        /// </summary>
        public IDisposable AcquireFinishToken(string tag = null)
        {
            var t = _finishGate.Acquire(tag);
            Debug.Log($"[Gate] AcquireFinishToken tag={tag} -> {(t == null ? "NULL" : t.GetType().Name)} pending={_finishGate.Pending}");
            return t;
        }

        public void RequestFinishCheck() => TryCompleteFinish();

        public void OnSignal(AbilityPhase phase, Unit source, Unit target, PhaseSignal signal)
        {
            if (phase == null || source == null || target == null)
                return;

            // ✅ Никаких Reset тут. Если сигнал пришёл не по текущей фазе — игнорируем.
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
                            // поздний коллбек от прошлой фазы — игнор
                            if (capturedVersion != _phaseVersion)
                                return;

                            TryCompleteFinish();
                        });
                }
            }

            // Можно оставить: дешево, gate/finishRequested защитят от спама
            TryCompleteFinish();
        }

        private void TryCompleteFinish()
        {
            if (_currentPhase == null)
                return;

            // Gate — главный фильтр
            if (!_finishGate.IsOpen)
                return;

            Action cb = _requestFinishCheck;
            
            if (cb == null)
                return;

            // Не чаще 1 раза в кадр (чтобы не спамить, но позволить "дозреть" условиям policy)
            if (_lastRequestFrame == Time.frameCount)
                return;

            _lastRequestFrame = Time.frameCount;
            cb.Invoke();
        }

    }
}