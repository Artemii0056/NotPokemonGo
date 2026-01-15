using System;
using Cinemachine;
using Services;
using Units;
using UnityEngine;

namespace Cameras
{
    public sealed class CinemachineCameraService : ICameraService
    {
        private readonly CinemachineBrain _brain;

        public CinemachineCameraService(CinemachineBrain brain)
        {
            _brain = brain;
        }

        public void FocusOnSource(Unit source, Unit target, float duration, Action onComplete)
        {
            throw new NotImplementedException();
        }

        public void FocusOnTarget(Unit source, Unit target, float duration, Action onComplete)
        {
            throw new NotImplementedException();
        }

        public void Reset(float duration, Action onComplete)
        {
            throw new NotImplementedException();
        }

        public void Play(Abilities.Configs.CameraCommand cmd, Unit source, Unit target, float blendTimeout, Action onComplete)
        {
            // если Cinemachine не настроен — не ломаем фазу
            if (_brain == null)
            {
                onComplete?.Invoke();
                return;
            }

            switch (cmd)
            {
                case Abilities.Configs.CameraCommand.FocusOnSource:
                    Enable(source, true);
                    Enable(target, false);
                    break;

                case Abilities.Configs.CameraCommand.FocusOnTarget:
                    Enable(source, false);
                    Enable(target, true);
                    break;

                case Abilities.Configs.CameraCommand.Reset:
                    Enable(source, false);
                    Enable(target, false);
                    break;

                default:
                    onComplete?.Invoke();
                    return;
            }

            // Если бленда нет — можно сразу завершать
            if (_brain.ActiveBlend == null)
            {
                onComplete?.Invoke();
                return;
            }

            // Вызов “когда можно продолжать” делает не handler, а сервис — по Rule A
            // Но нам нельзя корутины тут: значит используем таймер через Update-объект или tween.
            // Самый лёгкий способ без новых монобехов: delayed invoke через Unity.
            // Честно: это компромисс. Если хочешь “идеально” — вынесем в ITimer/CoroutineRunner.
            WaitBlendEndOrTimeout(blendTimeout, onComplete);
        }

        public void Shake(float intensity, float duration, Action onComplete)
        {
            throw new NotImplementedException();
        }

        private void Enable(Unit u, bool enabled)
        {
            if (u == null) return;
            if (u.virtualCamera == null) return;
            u.virtualCamera.enabled = enabled;
        }

        private void WaitBlendEndOrTimeout(float timeout, Action onComplete)
        {
            // Создаём маленький hidden runner один раз — самый быстрый способ.
            // Если у тебя уже есть ICoroutineRunner/ITimeService — скажи, сделаем через него.
            CameraServiceRunner.Instance.Run(_brain, Mathf.Max(0.01f, timeout), onComplete);
        }

        /// <summary>
        /// Внутренний “микро-раннер” для ожидания конца бленда без внедрения корутин в AbilityHandler.
        /// </summary>
        private sealed class CameraServiceRunner : MonoBehaviour
        {
            public static CameraServiceRunner Instance
            {
                get
                {
                    if (_instance != null) return _instance;
                    var go = new GameObject("[CameraServiceRunner]");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CameraServiceRunner>();
                    return _instance;
                }
            }
            private static CameraServiceRunner _instance;

            public void Run(CinemachineBrain brain, float timeout, Action onComplete)
            {
                StopAllCoroutines();
                StartCoroutine(Wait(brain, timeout, onComplete));
            }

            private System.Collections.IEnumerator Wait(CinemachineBrain brain, float timeout, Action onComplete)
            {
                float t = 0f;

                // ждём хотя бы кадр, чтобы ActiveBlend успел обновиться
                yield return null;

                while (brain != null && brain.ActiveBlend != null && t < timeout)
                {
                    t += Time.deltaTime;
                    yield return null;
                }

                onComplete?.Invoke();
            }
        }
    }
}
