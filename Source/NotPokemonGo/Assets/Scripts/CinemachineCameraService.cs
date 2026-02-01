using System;
using Cinemachine;
using Services;
using Units;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class CinemachineCameraService : ICameraService
{
    private const int ActivePriority = 20;
    private const int InactivePriority = 0;

    private readonly CinemachineBrain _brain;

    public CinemachineCameraService()
    {
        _brain = Object.FindFirstObjectByType<CinemachineBrain>();
    }

    public void Play(CameraCommand cmd, Unit source, Unit target, float blendTimeout, Action onComplete)
    {
        if (_brain == null)
        {
            onComplete?.Invoke();
            return;
        }
        
        switch (cmd)
        {
            
            case CameraCommand.FocusOnSource:
                SetPriority(source, ActivePriority);
                SetPriority(target, InactivePriority);
                break;

            case CameraCommand.FocusOnTarget:
                SetPriority(source, InactivePriority);
                SetPriority(target, ActivePriority);
                break;

            case CameraCommand.Reset:
                SetPriority(source, InactivePriority);
                SetPriority(target, InactivePriority);
                break;

            default:
                onComplete?.Invoke();
                return;
        }

        // ждём появления бленда (он появляется не в тот же кадр)
        CameraServiceRunner.Instance.Run(_brain, blendTimeout, onComplete);
    }

    private static void SetPriority(Unit unit, int priority)
    {
        if (unit == null) 
            return;
        //unit.virtualCamera.
        
        if (unit.virtualCamera == null) 
            return;

        unit.virtualCamera.Priority = priority;
    }

    private sealed class CameraServiceRunner : MonoBehaviour
    {
        private static CameraServiceRunner _instance;
        public static CameraServiceRunner Instance
        {
            get
            {
                if (_instance != null) 
                    return _instance;
                
                var go = new GameObject("[CameraServiceRunner]");
                
                DontDestroyOnLoad(go);
                
                _instance = go.AddComponent<CameraServiceRunner>();
                return _instance;
            }
        }

        public void Run(CinemachineBrain brain, float timeout, Action onComplete)
        {
            StopAllCoroutines();
            StartCoroutine(WaitBlend(brain, timeout, onComplete));
        }

        private System.Collections.IEnumerator WaitBlend(
            CinemachineBrain brain,
            float timeout,
            Action onComplete)
        {
            float t = 0f;

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

public enum CameraCommand
{
    None,
FocusOnSource,
FocusOnTarget,
Reset,
}
