using System;
using Effects;
using Pools;
using Services.Cameras;
using UnityEngine;

namespace Services.EffectViewServices
{
    public sealed class CombatTextPresenter : IDisposable
    {
        private readonly IEffectResolver _resolver;
        private readonly ICombatTextPool _pool;
        private readonly RectTransform _popupsRoot;
        private readonly Camera _camera;

        private readonly Vector3 _worldOffset = new(0f, 2f, 0f);

        public CombatTextPresenter(
            IEffectResolver resolver,
            ICombatTextPool pool,
            RectTransform popupsRoot,
            ICameraProvider provider)
        {
            _resolver = resolver;
            _pool = pool;
            _popupsRoot = popupsRoot;
            _camera = provider.Camera;
            
            _resolver.EffectApplied += OnEffectApplied;
            Debug.Log("[CombatTextPresenter] Started");
        }

        // public void Start()
        // {
        //     _resolver.EffectApplied += OnEffectApplied;
        //     Debug.Log("[CombatTextPresenter] Started");
        // }

        public void Dispose()
        {
            _resolver.EffectApplied -= OnEffectApplied;
        }

        private void OnEffectApplied(EffectResolver.EffectDataPayload payload)
        {
            if (payload.Target == null) 
                return;
            
            if (Math.Abs(payload.FinalValue) < Mathf.Epsilon) 
                return;

            var view = _pool.Get(_popupsRoot);

            var worldPos = payload.Target.transform.position + _worldOffset;
            
            var screenPos = _camera.WorldToScreenPoint(worldPos);
            
            if (screenPos.z <= 0f)
            {
                _pool.Return(view); 
                return;
                
            }

            var canvas = _popupsRoot.GetComponentInParent<Canvas>();
            
            var eventCam = canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas != null ? canvas.worldCamera : null;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _popupsRoot, screenPos, eventCam, out var localPoint);

            view.RectTransform.anchoredPosition = localPoint;

            view.Play(payload.FinalValue, payload.Effect.Type, () => _pool.Return(view));
        }
    }
}
